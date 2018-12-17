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
    public class ManagerService : IManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IManagerRepository _managerRepository;
        private readonly ISquadManagerRepository _squadManagerRepository;

        public ManagerService(IUnitOfWork unitOfWork,
            IManagerRepository managerRepository,
            ISquadManagerRepository squadManagerRepository)
        {
            _unitOfWork = unitOfWork;
            _managerRepository = managerRepository;
            _squadManagerRepository = squadManagerRepository;
        }

        public async Task<PaginatedList<Manager>> GetAsync(ManagerQuery managerQuery)
        {
            return await _managerRepository.GetAsync(managerQuery);
        }

        public async Task<IEnumerable<Manager>> GetBriefListAsync()
        {
            return await _managerRepository.GetBriefListAsync();
        }

        public async Task<Manager> GetByIdAsync(int id)
        {
            return await _managerRepository.GetAsync(id);
        }

        public async Task<Manager> GetDetailByIdAsync(int id)
        {
            return await _managerRepository.GetDetailAsync(id);
        }

        public async Task CreateAsync(Manager manager)
        {
            _managerRepository.Add(manager);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Manager manager)
        {
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Manager manager)
        {
            _managerRepository.Remove(manager);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<PaginatedList<SquadManager>> GetManagersInSquadAsync(SquadManagerQuery squadManagerQuery)
        {
            return await _squadManagerRepository.GetAsync(squadManagerQuery);
        }
    }
}