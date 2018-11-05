using System.Linq;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Repositories;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Persistence.Repositories
{
    public class ClubRepository : Repository<Club>, IClubRepository
    {
        public ClubRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Club>> GetAsync(ClubQuery clubQuery)
        {
            var query = Context.Clubs.AsQueryable();
            return await PaginatedList<Club>.CreateAsync(query, clubQuery.PageNumber, clubQuery.PageSize);
        }
    }
}