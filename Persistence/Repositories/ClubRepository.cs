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
    public class ClubRepository : Repository<Club>, IClubRepository
    {
        public ClubRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Club>> GetAsync(ClubQuery clubQuery)
        {
            var query = Context.Clubs
                .Include(c => c.Stadium)
                .AsQueryable();

            if (clubQuery.SeasonId.HasValue)
            {
                query = query.Where(c => c.SeasonClubs
                    .Any(sc => sc.SeasonId == clubQuery.SeasonId));
            }

            var columnsMap = new Dictionary<string, Expression<Func<Club, object>>>()
            {
                ["id"] = c => c.Id,
                ["code"] = c => c.Code,
                ["name"] = c => c.Name
            };

            query = query.Sort(clubQuery, columnsMap);

            return await PaginatedList<Club>.CreateAsync(query, clubQuery.PageNumber, clubQuery.PageSize);
        }

        public async Task<IEnumerable<Club>> GetBriefListAsync(int? seasonId)
        {
            var query = Context.Clubs.AsQueryable();

            if (seasonId.HasValue)
            {
                query = query.Where(c => c.SeasonClubs
                    .Any(sc => sc.SeasonId == seasonId));
            }

            return await query
                .OrderBy(c => c.Name)
                .Select(c => new Club
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<Club> GetDetailByIdAsync(int id)
        {
            return await Context.Clubs
                .Include(c => c.Stadium)
                .Include(c => c.Squads)
                .ThenInclude(s => s.Season)
                .Include(c => c.Squads)
                .ThenInclude(s => s.Kits)
                .SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}