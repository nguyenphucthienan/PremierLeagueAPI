using System.Threading.Tasks;

namespace PremierLeagueAPI.Core.Services
{
    public interface IMatchService
    {
        Task GenerateMatchesAsync();
    }
}