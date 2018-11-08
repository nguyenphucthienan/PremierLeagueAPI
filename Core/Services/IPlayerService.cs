using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Services
{
    public interface IPlayerService
    {
        Task<PaginatedList<Player>> GetAsync(PlayerQuery playerQuery);
        Task<Player> GetByIdAsync(int id);
        Task<Player> GetDetailByIdAsync(int id);
        Task CreateAsync(Player player);
        Task UpdateAsync(Player player);
        Task DeleteAsync(Player player);
    }
}