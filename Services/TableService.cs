using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Repositories;
using PremierLeagueAPI.Core.Services;

namespace PremierLeagueAPI.Services
{
    public class TableService : ITableService
    {
        private const int WinPoint = 3;
        private const int DrawPoint = 1;

        private readonly ISeasonRepository _seasonRepository;
        private readonly IClubRepository _clubRepository;

        public TableService(ISeasonRepository seasonRepository,
            IClubRepository clubRepository)
        {
            _seasonRepository = seasonRepository;
            _clubRepository = clubRepository;
        }

        public async Task<IEnumerable<TableItem>> GetAsync(int seasonId)
        {
            var season = await _seasonRepository.GetDetailAsync(seasonId);
            var clubsInSeason = season.SeasonClubs.Select(sc => sc.Club);

            var tableItems = new List<TableItem>();
            foreach (var club in clubsInSeason.ToList())
            {
                var clubDetail = await _clubRepository.GetDetailIncludeMatchesAsync(club.Id, seasonId);
                var playedHomeMatches = clubDetail.HomeMatches.Where(m => m.IsPlayed).ToList();
                var playedAwayMatches = clubDetail.AwayMatches.Where(m => m.IsPlayed).ToList();

                var played = playedHomeMatches.Count + playedAwayMatches.Count;

                var won = playedHomeMatches.Count(m => m.HomeClubId == club.Id && m.HomeScore > m.AwayScore)
                          + playedAwayMatches.Count(m => m.AwayClubId == club.Id && m.AwayScore > m.HomeScore);

                var drawn = playedHomeMatches.Count(m => m.HomeClubId == club.Id && m.HomeScore == m.AwayScore)
                            + playedAwayMatches.Count(m => m.AwayClubId == club.Id && m.AwayScore == m.HomeScore);

                var lost = playedHomeMatches.Count(m => m.HomeClubId == club.Id && m.HomeScore < m.AwayScore)
                           + playedAwayMatches.Count(m => m.AwayClubId == club.Id && m.AwayScore < m.HomeScore);

                var goalFor = playedHomeMatches.Count(m => m.Goals.Any(g => g.ClubId == club.Id))
                              + playedAwayMatches.Count(m => m.Goals.Any(g => g.ClubId == club.Id));

                var goalAgainst = playedHomeMatches.Count(m => m.Goals.Any(g => g.ClubId != club.Id))
                                  + playedAwayMatches.Count(m => m.Goals.Any(g => g.ClubId != club.Id));

                var point = WinPoint * won + DrawPoint * drawn;

                tableItems.Add(new TableItem
                {
                    Club = club,
                    Played = played,
                    Won = won,
                    Drawn = drawn,
                    Lost = lost,
                    GoalFor = goalFor,
                    GoalAgainst = goalAgainst,
                    GoalDifference = goalFor - goalAgainst,
                    Point = point
                });
            }

            tableItems = tableItems.OrderByDescending(e => e.Point)
                .ThenByDescending(e => e.GoalDifference)
                .ThenByDescending(e => e.GoalFor)
                .ThenBy(e => e.GoalAgainst)
                .ThenBy(e => e.Club.Name)
                .ToList();

            var rank = 1;
            foreach (var tableItem in tableItems)
            {
                tableItem.Rank = rank++;
            }

            return tableItems;
        }
    }
}