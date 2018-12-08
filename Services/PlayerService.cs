using System.Collections.Generic;
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
        private readonly ISquadPlayerRepository _squadPlayerRepository;

        public PlayerService(IUnitOfWork unitOfWork,
            IPlayerRepository playerRepository,
            ISquadPlayerRepository squadPlayerRepository)
        {
            _unitOfWork = unitOfWork;
            _playerRepository = playerRepository;
            _squadPlayerRepository = squadPlayerRepository;
        }

        public async Task<IEnumerable<Player>> GetBriefListAsync(int squadId)
        {
            return await _playerRepository.GetBriefListAsync(squadId);
        }

        public async Task<PaginatedList<Player>> GetAsync(PlayerQuery playerQuery)
        {
            return await _playerRepository.GetAsync(playerQuery);
        }

        public async Task<Player> GetByIdAsync(int id)
        {
            return await _playerRepository.GetAsync(id);
        }

        public async Task<Player> GetDetailByIdAsync(int id)
        {
            return await _playerRepository.GetDetailAsync(id);
        }

        public async Task CreateAsync(Player player)
        {
            _playerRepository.Add(player);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Player player)
        {
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Player player)
        {
            _playerRepository.Remove(player);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<PaginatedList<SquadPlayer>> GetPlayersInSquadAsync(SquadPlayerQuery squadPlayerQuery)
        {
            return await _squadPlayerRepository.GetAsync(squadPlayerQuery);
        }
    }
}