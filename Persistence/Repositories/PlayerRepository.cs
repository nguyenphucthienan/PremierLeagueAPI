using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public async Task<PaginatedList<Player>> GetByClubIdAsync(int clubId, PlayerQuery playerQuery)
        {
            var query = Context.Players
                .AsQueryable()
                .Where(p => p.ClubId == clubId);

            var columnsMap = new Dictionary<string, Expression<Func<Player, object>>>()
            {
                ["id"] = c => c.Id,
                ["name"] = c => c.Name
            };

            query = query.Sort(playerQuery, columnsMap);

            return await PaginatedList<Player>.CreateAsync(query, playerQuery.PageNumber, playerQuery.PageSize);
        }
    }
}