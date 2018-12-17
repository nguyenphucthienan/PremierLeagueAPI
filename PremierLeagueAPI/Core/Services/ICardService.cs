using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Core.Services
{
    public interface ICardService
    {
        Task<PaginatedList<Card>> GetAsync(CardQuery cardQuery);
        Task<Card> GetByIdAsync(int id);
        Task<Card> GetDetailByIdAsync(int id);
        Task CreateAsync(Card goal);
        Task UpdateAsync(Card goal);
        Task DeleteAsync(Card goal);
    }
}