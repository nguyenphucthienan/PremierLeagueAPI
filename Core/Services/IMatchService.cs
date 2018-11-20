using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Services
{
    public interface IMatchService
    {
        Task<PaginatedList<Match>> GetAsync(MatchQuery matchQuery);
        Task<Match> GetByIdAsync(int id);
        Task<Match> GetDetailByIdAsync(int id);
        Task GenerateAsync(int seasonId);
        Task UpdateAsync(Match match);
        Task DeleteAsync(Match match);
        Task<IEnumerable<int>> GetListRounds(int seasonId);
    }
}