using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface IKitRepository : IRepository<Kit>
    {
        Task<IEnumerable<Kit>> GetBySquadIdAsync(int squadId);
        Task<Kit> GetDetailAsync(int id);
    }
}