using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Services
{
    public interface IKitService
    {
        Task<PaginatedList<Kit>> GetAsync(KitQuery kitQuery);
        Task<IEnumerable<Kit>> GetBySquadIdAsync(int squadId);
        Task<IEnumerable<Kit>> GetBySeasonIdAndClubIdAsync(int seasonId, int clubId);
        Task<Kit> GetByIdAsync(int id);
        Task<Kit> GetDetailByIdAsync(int id);
        Task CreateAsync(Kit kit);
        Task UpdateAsync(Kit kit);
        Task DeleteAsync(Kit kit);
    }
}