using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Services
{
    public interface IStadiumService
    {
        Task<PaginatedList<Stadium>> GetAsync(StadiumQuery stadiumQuery);
        Task<IEnumerable<Stadium>> GetBriefListAsync();
        Task<Stadium> GetByIdAsync(int id);
        Task<Stadium> GetDetailByIdAsync(int id);
        Task CreateAsync(Stadium stadium);
        Task UpdateAsync(Stadium stadium);
        Task DeleteAsync(Stadium stadium);
    }
}