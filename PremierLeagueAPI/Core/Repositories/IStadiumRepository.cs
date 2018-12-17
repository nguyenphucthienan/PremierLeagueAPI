using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface IStadiumRepository : IRepository<Stadium>
    {
        Task<PaginatedList<Stadium>> GetAsync(StadiumQuery stadiumQuery);
        Task<IEnumerable<Stadium>> GetBriefListAsync();
        Task<Stadium> GetDetailAsync(int id);
    }
}