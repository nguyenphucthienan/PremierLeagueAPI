using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface IMatchRepository : IRepository<Match>
    {
        Task<PaginatedList<Match>> GetAsync(MatchQuery matchQuery);
        Task<Match> GetDetailByIdAsync(int id);
    }
}