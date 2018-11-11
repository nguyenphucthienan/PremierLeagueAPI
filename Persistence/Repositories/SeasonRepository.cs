using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Repositories;

namespace PremierLeagueAPI.Persistence.Repositories
{
    public class SeasonRepository : Repository<Season>, ISeasonRepository
    {
        public SeasonRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<Season> GetDetailAsync(int id)
        {
            return await Context.Seasons
                .Include(s => s.SeasonClubs)
                .ThenInclude(sc => sc.Club)
                .SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}