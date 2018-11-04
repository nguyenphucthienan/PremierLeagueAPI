using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Repositories;

namespace PremierLeagueAPI.Persistence.Repositories
{
    public class ClubRepository : Repository<Club>, IClubRepository
    {
        public ClubRepository(PremierLeagueDbContext context) : base(context)
        {
        }
    }
}