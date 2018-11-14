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
    public class SquadRepository : Repository<Squad>, ISquadRepository
    {
        public SquadRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Squad>> GetAsync(SquadQuery squadQuery)
        {
            var query = Context.Squads.AsQueryable();

            if (squadQuery.SeasonId.HasValue)
            {
                query = query.Where(s => s.SeasonId == squadQuery.SeasonId);
            }

            var columnsMap = new Dictionary<string, Expression<Func<Squad, object>>>()
            {
                ["id"] = s => s.Id
            };

            query = query.Sort(squadQuery, columnsMap);

            return await PaginatedList<Squad>.CreateAsync(query, squadQuery.PageNumber, squadQuery.PageSize);
        }

        public async Task<Squad> GetDetailAsync(int id)
        {
            return await Context.Squads
                .Include(s => s.Season)
                .Include(s => s.Club)
                .SingleOrDefaultAsync(s => s.Id == id);
        }
    }
}