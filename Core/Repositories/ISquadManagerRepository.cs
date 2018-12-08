using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface ISquadManagerRepository : IRepository<SquadManager>
    {
        Task<PaginatedList<SquadManager>> GetAsync(SquadManagerQuery squadManagerQuery);
    }
}