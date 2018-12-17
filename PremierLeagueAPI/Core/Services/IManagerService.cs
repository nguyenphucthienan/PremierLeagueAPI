using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Services
{
    public interface IManagerService
    {
        Task<PaginatedList<Manager>> GetAsync(ManagerQuery managerQuery);
        Task<IEnumerable<Manager>> GetBriefListAsync();
        Task<Manager> GetByIdAsync(int id);
        Task<Manager> GetDetailByIdAsync(int id);
        Task CreateAsync(Manager manager);
        Task UpdateAsync(Manager manager);
        Task DeleteAsync(Manager manager);
        Task<PaginatedList<SquadManager>> GetManagersInSquadAsync(SquadManagerQuery squadManagerQuery);
    }
}