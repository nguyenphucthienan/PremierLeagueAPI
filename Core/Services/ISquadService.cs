using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Services
{
    public interface ISquadService
    {
        Task<PaginatedList<Squad>> GetAsync(SquadQuery squadQuery);
        Task<Squad> GetByIdAsync(int id);
        Task<Squad> GetDetailByIdAsync(int id);
        Task CreateAsync(Squad squad);
        Task UpdateAsync(Squad squad);
        Task DeleteAsync(Squad squad);
    }
}