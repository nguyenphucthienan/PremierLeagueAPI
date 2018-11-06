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

        public async Task<Goal> GetDetailByIdAsync(int id)
        {
            return await Context.Goals
                .Include(g => g.Club)
                .Include(g => g.Match)
                .Include(g => g.Player)
                .SingleOrDefaultAsync(m => m.Id == id);
        }
    }
}