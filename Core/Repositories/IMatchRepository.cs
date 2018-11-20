using System.Collections.Generic;
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
        Task<IEnumerable<Match>> GetAllBySeasonIdAsync(int seasonId);
        Task<IEnumerable<int>> GetListRounds(int seasonId);
    }
}