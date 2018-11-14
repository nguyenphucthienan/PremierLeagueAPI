using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Repositories;
using PremierLeagueAPI.Core.Services;

namespace PremierLeagueAPI.Services
{
    public class KitService : IKitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKitRepository _kitRepository;

        public KitService(IUnitOfWork unitOfWork,
            IKitRepository kitRepository)
        {
            _unitOfWork = unitOfWork;
            _kitRepository = kitRepository;
        }

        public async Task<IEnumerable<Kit>> GetBySquadIdAsync(int squadId)
        {
            return await _kitRepository.GetBySquadIdAsync(squadId);
        }

        public async Task<Kit> GetByIdAsync(int id)
        {
            return await _kitRepository.GetAsync(id);
        }

        public async Task<Kit> GetDetailByIdAsync(int id)
        {
            return await _kitRepository.GetDetailAsync(id);
        }

        public async Task CreateAsync(Kit kit)
        {
            _kitRepository.Add(kit);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Kit kit)
        {
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Kit kit)
        {
            _kitRepository.Remove(kit);
            await _unitOfWork.CompleteAsync();
        }
    }
}