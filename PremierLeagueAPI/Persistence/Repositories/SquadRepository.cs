﻿using System;
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
    public class SquadRepository : Repository<Squad>, ISquadRepository
    {
        public SquadRepository(PremierLeagueDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Squad>> GetAsync(SquadQuery squadQuery)
        {
            var query = Context.Squads
                .Include(s => s.Season)
                .Include(s => s.Club)
                .AsQueryable();

            if (squadQuery.SeasonId.HasValue)
            {
                query = query.Where(s => s.SeasonId == squadQuery.SeasonId);
            }

            var columnsMap = new Dictionary<string, Expression<Func<Squad, object>>>()
            {
                ["id"] = s => s.Id,
                ["club"] = s => s.Club.Name
            };

            query = query.Sort(squadQuery, columnsMap);

            return await PaginatedList<Squad>.CreateAsync(query, squadQuery.PageNumber, squadQuery.PageSize);
        }

        public async Task<Squad> GetDetailAsync(int id)
        {
            return await Context.Squads
                .Include(s => s.SquadManagers)
                .Include(s => s.SquadPlayers)
                .Include(s => s.Season)
                .Include(s => s.Club)
                .Include(s => s.Kits)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Squad> GetDetailAsync(int seasonId, int clubId)
        {
            return await Context.Squads
                .Include(s => s.SquadManagers)
                .Include(s => s.SquadPlayers)
                .Include(s => s.Season)
                .Include(s => s.Club)
                .Include(s => s.Kits)
                .SingleOrDefaultAsync(s => s.SeasonId == seasonId && s.ClubId == clubId);
        }
    }
}