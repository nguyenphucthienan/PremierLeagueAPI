using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Repositories;

namespace PremierLeagueAPI.Persistence.Repositories
{
    public class GoalRepository : Repository<Goal>, IGoalRepository
    {
        public GoalRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Goal>> GetByMatchIdAsync(int matchId)
        {
            return await Context.Goals
                .Include(g => g.Club)
                .Include(g => g.Player)
                .Where(g => g.MatchId == matchId)
                .ToListAsync();
        }

        public async Task<Goal> GetDetailByIdAsync(int id)
        {
            return await Context.Goals
                .Include(g => g.Club)
                .Include(g => g.Match)
                .Include(g => g.Player)
                .SingleOrDefaultAsync(g => g.Id == id);
        }
    }
}