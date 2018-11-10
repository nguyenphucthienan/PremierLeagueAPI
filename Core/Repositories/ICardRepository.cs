using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Core.Repositories
{
    public interface ICardRepository : IRepository<Card>
    {
        Task<IEnumerable<Card>> GetByMatchIdAsync(int matchId);
        Task<Card> GetDetailByIdAsync(int id);
    }
}