using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface ISeasonRepository : IRepository<Season>
    {
        Task<Season> GetDetailAsync(int id);
    }
}