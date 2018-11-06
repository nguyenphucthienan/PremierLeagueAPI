using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Core.Services
{
    public interface IGoalService
    {
        Task<IEnumerable<Goal>> GetByMatchIdAsync(int matchId);
        Task<Goal> GetDetailByIdAsync(int id);
        Task CreateAsync(Goal goal);
    }
}