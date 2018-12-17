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
    public class ManagerRepository : Repository<Manager>, IManagerRepository
    {
        public ManagerRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Manager>> GetAsync(ManagerQuery managerQuery)
        {
            var query = Context.Managers
                .Include(m => m.SquadManagers)
                .AsQueryable();

            if (managerQuery.SquadId.HasValue)
            {
                query = query.Where(p => p.SquadManagers
                    .Any(sm => sm.SquadId == managerQuery.SquadId
                               && (sm.EndDate == null || sm.EndDate >= DateTime.Now)));
            }
            else if (managerQuery.SeasonId.HasValue && managerQuery.ClubId.HasValue)
            {
                query = query.Where(p => p.SquadManagers
                    .Any(sm => sm.Squad.SeasonId == managerQuery.SeasonId
                               && sm.Squad.ClubId == managerQuery.ClubId
                               && (sm.EndDate == null || sm.EndDate >= DateTime.Now)));
            }

            if (managerQuery.Name != null)
                query = query.Where(p => p.Name.Contains(managerQuery.Name));

            var columnsMap = new Dictionary<string, Expression<Func<Manager, object>>>()
            {
                ["id"] = m => m.Id,
                ["name"] = m => m.Name,
                ["nationality"] = m => m.Nationality,
                ["birthdate"] = m => m.Birthdate
            };

            query = query.Sort(managerQuery, columnsMap);

            return await PaginatedList<Manager>.CreateAsync(query, managerQuery.PageNumber, managerQuery.PageSize);
        }

        public async Task<IEnumerable<Manager>> GetBriefListAsync()
        {
            return await Context.Managers
                .ToListAsync();
        }

        public async Task<Manager> GetDetailAsync(int id)
        {
            return await Context.Managers
                .Include(p => p.SquadManagers)
                .ThenInclude(sm => sm.Squad)
                .ThenInclude(s => s.Season)
                .Include(p => p.SquadManagers)
                .ThenInclude(sm => sm.Squad)
                .ThenInclude(s => s.Club)
                .SingleOrDefaultAsync(m => m.Id == id);
        }
    }
}