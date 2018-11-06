using System;
using System.Collections.Generic;
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

        public MatchService(IUnitOfWork unitOfWork,
            IClubRepository clubRepository,
            IMatchRepository matchRepository)
        {
            _unitOfWork = unitOfWork;
            _clubRepository = clubRepository;
            _matchRepository = matchRepository;
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

        public async Task GenerateMatchesAsync()
        {
            var clubs = await _clubRepository.GetBriefListAsync();
            var clubCount = clubs.Count;
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

                    matches.Add(new Match
                    {
                        Round = round + 1,
                        HomeClubId = clubs[home].Id,
                        AwayClubId = clubs[away].Id,
                        MatchTime = matchTime,
                        IsPlayed = false
                    });

                    matches.Add(new Match
                    {
                        Round = round + 1 + roundCount,
                        HomeClubId = clubs[away].Id,
                        AwayClubId = clubs[home].Id,
                        MatchTime = matchTime.AddDays(roundCount * 7),
                        IsPlayed = false
                    });
                }

                matchTime = matchTime.AddDays(7);
            }

            _matchRepository.AddRange(matches);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteMatchAsync(Match match)
        {
            _matchRepository.Remove(match);
            await _unitOfWork.CompleteAsync();
        }
    }
}