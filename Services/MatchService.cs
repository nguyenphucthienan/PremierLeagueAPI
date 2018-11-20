using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PremierLeagueAPI.Core;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Repositories;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Services
{
    public class MatchService : IMatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClubRepository _clubRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IGoalRepository _goalRepository;

        public MatchService(IUnitOfWork unitOfWork,
            IClubRepository clubRepository,
            IMatchRepository matchRepository,
            IGoalRepository goalRepository
        )
        {
            _unitOfWork = unitOfWork;
            _clubRepository = clubRepository;
            _matchRepository = matchRepository;
            _goalRepository = goalRepository;
        }

        public async Task<PaginatedList<Match>> GetAsync(MatchQuery matchQuery)
        {
            return await _matchRepository.GetAsync(matchQuery);
        }

        public async Task<Match> GetByIdAsync(int id)
        {
            return await _matchRepository.GetAsync(id);
        }

        public async Task<Match> GetDetailByIdAsync(int id)
        {
            return await _matchRepository.GetDetailByIdAsync(id);
        }

        public async Task GenerateAsync(int seasonId)
        {
            var clubs = await _clubRepository.GetBriefListAsync(seasonId);
            var clubList = clubs.ToList();
            var clubCount = clubList.Count;
            var roundCount = clubCount - 1;
            var matchesPerRound = clubCount / 2;

            var matchTime = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                17, 00, 00);

            var matches = new List<Match>();

            for (var round = 0; round < roundCount; round++)
            {
                for (var match = 0; match < matchesPerRound; match++)
                {
                    var home = (round + match) % (clubCount - 1);
                    var away = (clubCount - 1 - match + round) % (clubCount - 1);

                    if (match == 0)
                        away = clubCount - 1;

                    var homeClub = await _clubRepository.GetAsync(clubList[home].Id);
                    var awayClub = await _clubRepository.GetAsync(clubList[away].Id);

                    matches.Add(new Match
                    {
                        SeasonId = seasonId,
                        Round = round + 1,
                        StadiumId = homeClub.StadiumId,
                        HomeClubId = clubList[home].Id,
                        AwayClubId = clubList[away].Id,
                        MatchTime = matchTime,
                        IsPlayed = false
                    });

                    matches.Add(new Match
                    {
                        SeasonId = seasonId,
                        Round = round + 1 + roundCount,
                        StadiumId = awayClub.StadiumId,
                        HomeClubId = clubList[away].Id,
                        AwayClubId = clubList[home].Id,
                        MatchTime = matchTime.AddDays(roundCount * 7),
                        IsPlayed = false
                    });
                }

                matchTime = matchTime.AddDays(7);
            }

            _matchRepository.AddRange(matches);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAllAsync(int seasonId)
        {
            var matches = await _matchRepository.GetAllBySeasonIdAsync(seasonId);
            _matchRepository.RemoveRange(matches);

            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Match match)
        {
            if (match.IsPlayed == false)
            {
                _goalRepository.RemoveRange(match.Goals);
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Match match)
        {
            _matchRepository.Remove(match);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<int>> GetListRounds(int seasonId)
        {
            return await _matchRepository.GetListRounds(seasonId);
        }
    }
}