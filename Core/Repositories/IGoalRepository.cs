using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface IGoalRepository : IRepository<Goal>
    {
        Task<IEnumerable<Goal>> GetByMatchIdAsync(int matchId);
        Task<Goal> GetDetailByIdAsync(int id);
    }
}