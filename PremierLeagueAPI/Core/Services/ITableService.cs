using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Core.Services
{
    public interface ITableService
    {
        Task<IEnumerable<TableItem>> GetAsync(int seasonId);
    }
}