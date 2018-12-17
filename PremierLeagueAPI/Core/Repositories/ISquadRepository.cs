using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface ISquadRepository : IRepository<Squad>
    {
        Task<PaginatedList<Squad>> GetAsync(SquadQuery squadQuery);
        Task<Squad> GetDetailAsync(int id);
        Task<Squad> GetDetailAsync(int seasonId, int squadId);
    }
}