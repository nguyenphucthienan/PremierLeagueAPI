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
    public class SquadPlayerRepository : Repository<SquadPlayer>, ISquadPlayerRepository
    {
        public SquadPlayerRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<SquadPlayer>> GetAsync(SquadPlayerQuery squadPlayerQuery)
        {
            var query = Context.SquadPlayers
                .Include(sp => sp.Squad)
                .Include(sp => sp.Player)
                .Where(sp => sp.SquadId == squadPlayerQuery.SquadId)
                .AsQueryable();

            if (squadPlayerQuery.Name != null)
                query = query.Where(sp => sp.Player.Name.Contains(squadPlayerQuery.Name));

            if (squadPlayerQuery.PositionType.HasValue)
                query = query.Where(sp => sp.Player.PositionType == squadPlayerQuery.PositionType);

            var columnsMap = new Dictionary<string, Expression<Func<SquadPlayer, object>>>()
            {
                ["id"] = sp => sp.Player.Id,
                ["name"] = sp => sp.Player.Name,
                ["positionType"] = sp => sp.Player.PositionType,
                ["nationality"] = sp => sp.Player.Nationality,
                ["birthdate"] = sp => sp.Player.Birthdate,
                ["height"] = sp => sp.Player.Height,
                ["weight"] = sp => sp.Player.Weight,
                ["number"] = sp => sp.Number,
                ["startDate"] = sp => sp.StartDate,
                ["endDate"] = sp => sp.EndDate
            };

            query = query.Sort(squadPlayerQuery, columnsMap);

            return await PaginatedList<SquadPlayer>.CreateAsync(query, squadPlayerQuery.PageNumber,
                squadPlayerQuery.PageSize);
        }
    }
}