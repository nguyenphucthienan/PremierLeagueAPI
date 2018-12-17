using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Services
{
    public interface ISeasonService
    {
        Task<PaginatedList<Season>> GetAsync(SeasonQuery seasonQuery);
        Task<IEnumerable<Season>> GetBriefListAsync();
        Task<Season> GetByIdAsync(int id);
        Task<Season> GetDetailByIdAsync(int id);
        Task CreateAsync(Season season);
        Task UpdateAsync(Season season);
        Task DeleteAsync(Season season);
    }
}