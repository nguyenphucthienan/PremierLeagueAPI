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
    public class StadiumRepository : Repository<Stadium>, IStadiumRepository
    {
        public StadiumRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Stadium>> GetAsync(StadiumQuery stadiumQuery)
        {
            var query = Context.Stadiums.AsQueryable();

            var columnsMap = new Dictionary<string, Expression<Func<Stadium, object>>>()
            {
                ["id"] = s => s.Id,
                ["name"] = s => s.Name
            };

            query = query.Sort(stadiumQuery, columnsMap);

            return await PaginatedList<Stadium>.CreateAsync(query, stadiumQuery.PageNumber, stadiumQuery.PageSize);
        }

        public async Task<IEnumerable<Stadium>> GetBriefListAsync()
        {
            return await Context.Stadiums
                .AsQueryable()
                .OrderBy(s => s.Name)
                .Select(s => new Stadium
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync();
        }

        public async Task<Stadium> GetDetailAsync(int id)
        {
            return await Context.Stadiums
                .Include(s => s.Clubs)
                .SingleOrDefaultAsync(s => s.Id == id);
        }
    }
}