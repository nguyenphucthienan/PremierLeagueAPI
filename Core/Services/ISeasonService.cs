using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Core.Services
{
    public interface ISeasonService
    {
        Task<IEnumerable<Season>> GetAllAsync();
        Task<IEnumerable<Season>> GetBriefListAsync();
        Task<Season> GetByIdAsync(int id);
        Task<Season> GetDetailByIdAsync(int id);
        Task CreateAsync(Season season);
        Task UpdateAsync(Season season);
        Task DeleteAsync(Season season);
    }
}