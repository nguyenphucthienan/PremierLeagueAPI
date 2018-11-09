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
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        public PlayerRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Player>> GetAsync(PlayerQuery playerQuery)
        {
            var query = Context.Players
                .Include(p => p.Club)
                .AsQueryable();

            if (playerQuery.ClubId.HasValue)
                query = query.Where(p => p.ClubId == playerQuery.ClubId);

            if (playerQuery.Position != null)
                query = query.Where(p => p.Position.ToLower() == playerQuery.Position);

            var columnsMap = new Dictionary<string, Expression<Func<Player, object>>>()
            {
                ["id"] = p => p.Id,
                ["name"] = p => p.Name,
                ["number"] = p => p.Number
            };

            query = query.Sort(playerQuery, columnsMap);

            return await PaginatedList<Player>.CreateAsync(query, playerQuery.PageNumber, playerQuery.PageSize);
        }

        public async Task<Player> GetDetailAsync(int id)
        {
            return await Context.Players
                .Include(p => p.Club)
                .SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}