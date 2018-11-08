using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface IPlayerRepository : IRepository<Player>
    {
        Task<PaginatedList<Player>> GetAsync(PlayerQuery playerQuery);
        Task<Player> GetDetailAsync(int id);
    }
}