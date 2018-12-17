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
    public class SquadManagerRepository : Repository<SquadManager>, ISquadManagerRepository
    {
        public SquadManagerRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<SquadManager>> GetAsync(SquadManagerQuery squadManagerQuery)
        {
            var query = Context.SquadManagers
                .Include(sm => sm.Squad)
                .Include(sm => sm.Manager)
                .Where(sp => sp.SquadId == squadManagerQuery.SquadId)
                .AsQueryable();

            if (squadManagerQuery.IsActive)
                query = query.Where(sp => sp.EndDate == null || sp.EndDate > DateTime.Now);

            if (squadManagerQuery.Name != null)
                query = query.Where(sp => sp.Manager.Name.Contains(squadManagerQuery.Name));

            var columnsMap = new Dictionary<string, Expression<Func<SquadManager, object>>>()
            {
                ["id"] = sp => sp.Manager.Id,
                ["name"] = sp => sp.Manager.Name,
                ["nationality"] = sp => sp.Manager.Nationality,
                ["birthdate"] = sp => sp.Manager.Birthdate,
                ["startDate"] = sp => sp.StartDate,
                ["endDate"] = sp => sp.EndDate
            };

            query = query.Sort(squadManagerQuery, columnsMap);

            return await PaginatedList<SquadManager>.CreateAsync(query, squadManagerQuery.PageNumber,
                squadManagerQuery.PageSize);
        }
    }
}