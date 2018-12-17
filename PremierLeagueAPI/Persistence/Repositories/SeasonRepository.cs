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
    public class SeasonRepository : Repository<Season>, ISeasonRepository
    {
        public SeasonRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Season>> GetAsync(SeasonQuery seasonQuery)
        {
            var query = Context.Seasons.AsQueryable();

            var columnsMap = new Dictionary<string, Expression<Func<Season, object>>>()
            {
                ["id"] = s => s.Id,
                ["name"] = s => s.Name
            };

            query = query.Sort(seasonQuery, columnsMap);

            return await PaginatedList<Season>.CreateAsync(query, seasonQuery.PageNumber, seasonQuery.PageSize);
        }

        public async Task<IEnumerable<Season>> GetBriefListAsync()
        {
            return await Context.Seasons
                .AsQueryable()
                .OrderBy(s => s.Name)
                .Select(s => new Season
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync();
        }

        public async Task<Season> GetDetailAsync(int id)
        {
            return await Context.Seasons
                .Include(s => s.Squads)
                .ThenInclude(ss => ss.Club)
                .SingleOrDefaultAsync(s => s.Id == id);
        }
    }
}