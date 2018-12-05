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
    public class KitRepository : Repository<Kit>, IKitRepository
    {
        public KitRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Kit>> GetAsync(KitQuery kitQuery)
        {
            var query = Context.Kits
                .Include(k => k.Squad)
                .AsQueryable();

            if (kitQuery.SquadId.HasValue)
            {
                query = query.Where(k => k.SquadId == kitQuery.SquadId);
            }
            else if (kitQuery.SeasonId.HasValue && kitQuery.ClubId.HasValue)
            {
                query = query.Where(k => k.Squad.SeasonId == kitQuery.SeasonId
                                         && k.Squad.ClubId == kitQuery.ClubId);
            }

            var columnsMap = new Dictionary<string, Expression<Func<Kit, object>>>()
            {
                ["id"] = p => p.Id
            };

            query = query.Sort(kitQuery, columnsMap);

            return await PaginatedList<Kit>.CreateAsync(query, kitQuery.PageNumber, kitQuery.PageSize);
        }

        public async Task<IEnumerable<Kit>> GetBySquadIdAsync(int squadId)
        {
            return await Context.Kits
                .Where(k => k.SquadId == squadId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Kit>> GetBySeasonIdAndClubIdAsync(int seasonId, int clubId)
        {
            return await Context.Kits
                .Where(k => k.Squad.SeasonId == seasonId
                            && k.Squad.ClubId == clubId)
                .ToListAsync();
        }

        public async Task<Kit> GetDetailAsync(int id)
        {
            return await Context.Kits
                .Include(k => k.Squad)
                .SingleOrDefaultAsync(k => k.Id == id);
        }

        public async Task<Kit> GetBySquadIdAndKitTypeAsync(int squadId, KitType kitType)
        {
            return await Context.Kits
                .SingleOrDefaultAsync(k => k.Squad.Id == squadId
                                           && k.KitType == kitType);
        }
    }
}