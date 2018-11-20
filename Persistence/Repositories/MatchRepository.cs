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
                .Include(m => m.Stadium)
                .Include(m => m.HomeClub)
                .Include(m => m.AwayClub)
                .AsQueryable();

            if (matchQuery.Round.HasValue)
                query = query.Where(m => m.Round == matchQuery.Round);

            if (matchQuery.IsPlayed.HasValue)
                query = query.Where(m => m.IsPlayed == matchQuery.IsPlayed);

            if (matchQuery.SeasonId.HasValue)
                query = query.Where(m => m.SeasonId == matchQuery.SeasonId);

            var columnsMap = new Dictionary<string, Expression<Func<Match, object>>>()
            {
                ["id"] = m => m.Id,
                ["round"] = m => m.Round,
                ["homeClub"] = m => m.HomeClub.Name,
                ["awayClub"] = m => m.AwayClub.Name,
                ["matchTime"] = m => m.MatchTime,
                ["isPlayed"] = m => m.IsPlayed,
                ["stadium"] = m => m.Stadium.Name
            };

            query = query.Sort(matchQuery, columnsMap);

            return await PaginatedList<Match>.CreateAsync(query, matchQuery.PageNumber, matchQuery.PageSize);
        }

        public async Task<Match> GetDetailByIdAsync(int id)
        {
            return await Context.Matches
                .Include(m => m.Stadium)
                .Include(m => m.HomeClub)
                .Include(m => m.AwayClub)
                .Include(m => m.Goals)
                .ThenInclude(g => g.Player)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<int>> GetListRounds(int seasonId)
        {
            return await Context.Matches
                .Where(m => m.SeasonId == seasonId)
                .Select(m => m.Round)
                .Distinct()
                .ToListAsync();
        }
    }
}