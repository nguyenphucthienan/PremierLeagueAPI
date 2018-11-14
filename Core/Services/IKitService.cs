using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Core.Services
{
    public interface IKitService
    {
        Task<IEnumerable<Kit>> GetBySquadIdAsync(int squadId);
        Task<Kit> GetByIdAsync(int id);
        Task<Kit> GetDetailByIdAsync(int id);
        Task CreateAsync(Kit kit);
        Task UpdateAsync(Kit kit);
        Task DeleteAsync(Kit kit);
    }
}