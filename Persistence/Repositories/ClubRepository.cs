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
    public class ClubRepository : Repository<Club>, IClubRepository
    {
        public ClubRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Club>> GetAsync(ClubQuery clubQuery)
        {
            var query = Context.Clubs.AsQueryable();

            var columnsMap = new Dictionary<string, Expression<Func<Club, object>>>()
            {
                ["id"] = c => c.Id,
                ["code"] = c => c.Code,
                ["name"] = c => c.Name
            };

            query = query.Sort(clubQuery, columnsMap);

            return await PaginatedList<Club>.CreateAsync(query, clubQuery.PageNumber, clubQuery.PageSize);
        }
    }
}