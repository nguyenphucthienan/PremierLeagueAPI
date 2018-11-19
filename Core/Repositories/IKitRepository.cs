﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface IKitRepository : IRepository<Kit>
    {
        Task<PaginatedList<Kit>> GetAsync(KitQuery kitQuery);
        Task<IEnumerable<Kit>> GetBySquadIdAsync(int squadId);
        Task<Kit> GetDetailAsync(int id);
    }
}