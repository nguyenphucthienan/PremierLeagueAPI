using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Services
{
    public interface IGoalService
    {
        Task<PaginatedList<Goal>> GetAsync(GoalQuery goalQuery);
        Task<Goal> GetByIdAsync(int id);
        Task<Goal> GetDetailByIdAsync(int id);
        Task CreateAsync(Goal goal);
        Task UpdateAsync(Goal goal);
        Task DeleteAsync(Goal goal);
    }
}