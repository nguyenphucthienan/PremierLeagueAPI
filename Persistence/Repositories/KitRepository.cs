using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Repositories;

namespace PremierLeagueAPI.Persistence.Repositories
{
    public class KitRepository : Repository<Kit>, IKitRepository
    {
        public KitRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Kit>> GetBySquadIdAsync(int squadId)
        {
            return await Context.Kits
                .Where(k => k.SquadId == squadId)
                .ToListAsync();
        }

        public async Task<Kit> GetDetailAsync(int id)
        {
            return await Context.Kits
                .Include(k => k.Squad)
                .SingleOrDefaultAsync(k => k.Id == id);
        }
    }
}