using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface IManagerRepository : IRepository<Manager>
    {
        Task<PaginatedList<Manager>> GetAsync(ManagerQuery managerQuery);
        Task<IEnumerable<Manager>> GetBriefListAsync();
        Task<Manager> GetDetailAsync(int id);
    }
}