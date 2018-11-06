using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface IGoalRepository : IRepository<Goal>
    {
        Task<Goal> GetDetailByIdAsync(int id);
    }
}