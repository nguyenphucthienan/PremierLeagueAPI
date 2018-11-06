using System.Threading.Tasks;
using PremierLeagueAPI.Core;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Repositories;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IUnitOfWork unitOfWork,
            IPlayerRepository playerRepository)
        {
            _unitOfWork = unitOfWork;
            _playerRepository = playerRepository;
        }

        public async Task<PaginatedList<Player>> GetByClubIdAsync(int clubId, PlayerQuery playerQuery)
        {
            return await _playerRepository.GetByClubIdAsync(clubId, playerQuery);
        }

        public async Task<Player> GetByIdAsync(int id)
        {
            return await _playerRepository.GetAsync(id);
        }
    }
}