using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface IClubRepository : IRepository<Club>
    {
        Task<PaginatedList<Club>> GetAsync(ClubQuery clubQuery);
        Task<IEnumerable<Club>> GetBriefListAsync(int? seasonId);
        Task<Club> GetDetailByIdAsync(int id);
        Task<Club> GetDetailIncludeMatchesAsync(int id, int seasonId);
    }
}