using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface ISquadPlayerRepository : IRepository<SquadPlayer>
    {
        Task<PaginatedList<SquadPlayer>> GetAsync(SquadPlayerQuery squadPlayerQuery);
    }
}