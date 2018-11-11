using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Repositories;

namespace PremierLeagueAPI.Persistence.Repositories
{
    public class SeasonRepository : Repository<Season>, ISeasonRepository
    {
        public SeasonRepository(PremierLeagueDbContext context) : base(context)
        {
        }
    }
}