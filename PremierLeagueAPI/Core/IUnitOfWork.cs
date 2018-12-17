using System.Threading.Tasks;

namespace PremierLeagueAPI.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}