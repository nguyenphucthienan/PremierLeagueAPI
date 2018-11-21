using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface IGoalRepository : IRepository<Goal>
    {
        Task<PaginatedList<Goal>> GetAsync(GoalQuery goalQuery);
        Task<Goal> GetDetailByIdAsync(int id);
    }
}