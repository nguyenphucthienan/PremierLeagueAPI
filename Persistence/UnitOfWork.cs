using System.Threading.Tasks;
using PremierLeagueAPI.Core;

namespace PremierLeagueAPI.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PremierLeagueDbContext _context;

        public UnitOfWork(PremierLeagueDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}