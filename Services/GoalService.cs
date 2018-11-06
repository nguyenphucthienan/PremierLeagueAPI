using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Repositories;
using PremierLeagueAPI.Core.Services;

namespace PremierLeagueAPI.Services
{
    public class GoalService : IGoalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGoalRepository _goalRepository;

        public GoalService(IUnitOfWork unitOfWork,
            IGoalRepository goalRepository)
        {
            _unitOfWork = unitOfWork;
            _goalRepository = goalRepository;
        }

        public async Task<IEnumerable<Goal>> GetByMatchIdAsync(int matchId)
        {
            return await _goalRepository.GetByMatchIdAsync(matchId);
        }

        public async Task<Goal> GetDetailByIdAsync(int id)
        {
            return await _goalRepository.GetDetailByIdAsync(id);
        }

        public async Task CreateGoal(Goal goal)
        {
            _goalRepository.Add(goal);
            await _unitOfWork.CompleteAsync();
        }
    }
}