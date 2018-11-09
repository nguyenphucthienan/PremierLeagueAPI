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
    public class MatchRepository : Repository<Match>, IMatchRepository
    {
        public MatchRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Match>> GetAsync(MatchQuery matchQuery)
        {
            var query = Context.Matches
                .Include(m => m.HomeClub)
                .Include(m => m.AwayClub)
                .AsQueryable();

            if (matchQuery.Round.HasValue)
                query = query.Where(m => m.Round == matchQuery.Round);

            var columnsMap = new Dictionary<string, Expression<Func<Match, object>>>()
            {
                ["id"] = m => m.Id,
                ["round"] = m => m.Round,
                ["matchTime"] = m => m.MatchTime
            };

            query = query.Sort(matchQuery, columnsMap);

            return await PaginatedList<Match>.CreateAsync(query, matchQuery.PageNumber, matchQuery.PageSize);
        }

        public async Task<Match> GetDetailByIdAsync(int id)
        {
            return await Context.Matches
                .Include(m => m.HomeClub)
                .Include(m => m.AwayClub)
                .Include(m => m.Goals)
                .SingleOrDefaultAsync(m => m.Id == id);
        }
    }
}