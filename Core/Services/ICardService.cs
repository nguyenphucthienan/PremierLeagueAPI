using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Core.Services
{
    public interface ICardService
    {
        Task<IEnumerable<Card>> GetByMatchIdAsync(int matchId);
        Task<Card> GetByIdAsync(int id);
        Task<Card> GetDetailByIdAsync(int id);
        Task CreateAsync(Card goal);
        Task UpdateAsync(Card goal);
        Task DeleteAsync(Card goal);
    }
}