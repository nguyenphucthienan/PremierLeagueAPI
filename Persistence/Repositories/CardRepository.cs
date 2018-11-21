using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Repositories;
using PremierLeagueAPI.Extensions;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Persistence.Repositories
{
    public class CardRepository : Repository<Card>, ICardRepository
    {
        public CardRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Card>> GetAsync(CardQuery cardQuery)
        {
            var query = Context.Cards
                .Include(g => g.Club)
                .Include(g => g.Player)
                .AsQueryable();

            if (cardQuery.MatchId.HasValue)
                query = query.Where(g => g.MatchId == cardQuery.MatchId);

            var columnsMap = new Dictionary<string, Expression<Func<Card, object>>>()
            {
                ["id"] = c => c.Id,
                ["club"] = c => c.Club.Name,
                ["player"] = c => c.Player.Name,
                ["cardType"] = c => c.CardType,
                ["cardTime"] = c => c.CardTime,
            };

            query = query.Sort(cardQuery, columnsMap);

            return await PaginatedList<Card>.CreateAsync(query, cardQuery.PageNumber, cardQuery.PageSize);
        }

        public async Task<Card> GetDetailByIdAsync(int id)
        {
            return await Context.Cards
                .Include(g => g.Match).ThenInclude(m => m.HomeClub)
                .Include(g => g.Match).ThenInclude(m => m.AwayClub)
                .Include(g => g.Match).ThenInclude(m => m.Stadium)
                .Include(g => g.Club)
                .Include(g => g.Player)
                .SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}