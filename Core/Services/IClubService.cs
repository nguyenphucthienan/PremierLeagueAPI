using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Services
{
    public interface IClubService
    {
        Task<PaginatedList<Club>> GetAsync(ClubQuery clubQuery);
        Task<Club> GetByIdAsync(int id);
        Task CreateAsync(Club club);
        Task UpdateAsync(Club club);
        Task DeleteAsync(Club club);
    }
}