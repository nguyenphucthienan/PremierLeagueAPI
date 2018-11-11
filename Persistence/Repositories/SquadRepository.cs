using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Repositories;

namespace PremierLeagueAPI.Persistence.Repositories
{
    public class SquadRepository : Repository<Squad>, ISquadRepository
    {
        public SquadRepository(PremierLeagueDbContext context) : base(context)
        {
        }
    }
}