﻿using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface IClubRepository : IRepository<Club>
    {
        Task<PaginatedList<Club>> GetAsync(ClubQuery clubQuery);
    }
}