using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Core.Services
{
    public interface IGoalService
    {
        Task<IEnumerable<Goal>> GetByMatchIdAsync(int matchId);
        Task<Goal> GetByIdAsync(int id);
        Task<Goal> GetDetailByIdAsync(int id);
        Task CreateAsync(Goal goal);
        Task UpdateAsync(Goal goal);
        Task DeleteAsync(Goal goal);
    }
}