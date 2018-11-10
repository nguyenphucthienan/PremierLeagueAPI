using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Repositories;

namespace PremierLeagueAPI.Persistence.Repositories
{
    public class CardRepository : Repository<Card>, ICardRepository
    {
        public CardRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Card>> GetByMatchIdAsync(int matchId)
        {
            return await Context.Cards
                .Include(g => g.Club)
                .Include(g => g.Player)
                .Where(g => g.MatchId == matchId)
                .ToListAsync();
        }

        public async Task<Card> GetDetailByIdAsync(int id)
        {
            return await Context.Cards
                .Include(g => g.Club)
                .Include(g => g.Match)
                .Include(g => g.Player)
                .SingleOrDefaultAsync(g => g.Id == id);
        }
    }
}