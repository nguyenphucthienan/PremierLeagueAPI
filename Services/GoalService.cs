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
    public class GoalService : IGoalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMatchRepository _matchRepository;
        private readonly IGoalRepository _goalRepository;

        public GoalService(IUnitOfWork unitOfWork,
            IMatchRepository matchRepository,
            IGoalRepository goalRepository)
        {
            _unitOfWork = unitOfWork;
            _matchRepository = matchRepository;
            _goalRepository = goalRepository;
        }

        public async Task<PaginatedList<Goal>> GetAsync(GoalQuery goalQuery)
        {
            return await _goalRepository.GetAsync(goalQuery);
        }

        public async Task<Goal> GetByIdAsync(int id)
        {
            return await _goalRepository.GetAsync(id);
        }

        public async Task<Goal> GetDetailByIdAsync(int id)
        {
            return await _goalRepository.GetDetailByIdAsync(id);
        }

        public async Task CreateAsync(Goal goal)
        {
            _goalRepository.Add(goal);

            var match = await _matchRepository.GetAsync(goal.MatchId);
            match.IsPlayed = true;

            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Goal goal)
        {
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Goal goal)
        {
            _goalRepository.Remove(goal);
            await _unitOfWork.CompleteAsync();
        }
    }
}