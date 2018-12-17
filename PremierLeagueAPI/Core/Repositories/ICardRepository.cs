using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface ICardRepository : IRepository<Card>
    {
        Task<PaginatedList<Card>> GetAsync(CardQuery cardQuery);
        Task<Card> GetDetailByIdAsync(int id);
    }
}