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
    public class StadiumService : IStadiumService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStadiumRepository _stadiumRepository;

        public StadiumService(IUnitOfWork unitOfWork,
            IStadiumRepository stadiumRepository)
        {
            _unitOfWork = unitOfWork;
            _stadiumRepository = stadiumRepository;
        }

        public async Task<PaginatedList<Stadium>> GetAsync(StadiumQuery stadiumQuery)
        {
            return await _stadiumRepository.GetAsync(stadiumQuery);
        }

        public async Task<IEnumerable<Stadium>> GetBriefListAsync()
        {
            return await _stadiumRepository.GetBriefListAsync();
        }

        public async Task<Stadium> GetByIdAsync(int id)
        {
            return await _stadiumRepository.GetAsync(id);
        }

        public async Task<Stadium> GetDetailByIdAsync(int id)
        {
            return await _stadiumRepository.GetDetailAsync(id);
        }

        public async Task CreateAsync(Stadium stadium)
        {
            _stadiumRepository.Add(stadium);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Stadium stadium)
        {
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Stadium stadium)
        {
            _stadiumRepository.Remove(stadium);
            await _unitOfWork.CompleteAsync();
        }
    }
}