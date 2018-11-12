using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface ISeasonRepository : IRepository<Season>
    {
        Task<PaginatedList<Season>> GetAsync(SeasonQuery seasonQuery);
        Task<IEnumerable<Season>> GetBriefListAsync();
        Task<Season> GetDetailAsync(int id);
    }
}