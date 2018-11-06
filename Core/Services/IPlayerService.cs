using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Services
{
    public interface IPlayerService
    {
        Task<PaginatedList<Player>> GetByClubIdAsync(int clubId, PlayerQuery playerQuery);
        Task<Player> GetByIdAsync(int id);
        Task<Player> CreatePlayer(Player player);
    }
}