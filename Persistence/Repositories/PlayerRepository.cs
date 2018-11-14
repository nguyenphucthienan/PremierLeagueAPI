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
                .Include(p => p.SquadPlayers)
                .AsQueryable();

            if (playerQuery.SquadId.HasValue)
            {
                query = query.Where(p => p.SquadPlayers
                    .Any(sp => sp.SquadId == playerQuery.SquadId));
            }
            else if (playerQuery.SeasonId.HasValue && playerQuery.ClubId.HasValue)
            {
                query = query.Where(p => p.SquadPlayers
                    .Any(sp => sp.Squad.SeasonId == playerQuery.SeasonId
                               && sp.Squad.ClubId == playerQuery.ClubId
                               && (sp.EndDate == null || sp.EndDate >= DateTime.Now)));
            }

            if (playerQuery.PositionType.HasValue)
                query = query.Where(p => p.PositionType == playerQuery.PositionType);

            var columnsMap = new Dictionary<string, Expression<Func<Player, object>>>()
            {
                ["id"] = p => p.Id,
                ["name"] = p => p.Name,
                ["position"] = p => p.PositionType
            };

            query = query.Sort(playerQuery, columnsMap);

            return await PaginatedList<Player>.CreateAsync(query, playerQuery.PageNumber, playerQuery.PageSize);
        }

        public async Task<Player> GetDetailAsync(int id)
        {
            return await Context.Players
                .Include(p => p.SquadPlayers)
                .ThenInclude(sp => sp.Squad)
                .ThenInclude(s => s.Season)
                .Include(p => p.SquadPlayers)
                .ThenInclude(sp => sp.Squad)
                .ThenInclude(s => s.Club)
                .SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}