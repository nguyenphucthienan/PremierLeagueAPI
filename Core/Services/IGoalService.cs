using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Core.Services
{
    public interface IGoalService
    {
        Task<Goal> GetDetailByIdAsync(int id);
        Task CreateGoal(Goal goal);
    }
}