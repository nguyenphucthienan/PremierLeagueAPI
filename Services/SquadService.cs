using System.Threading.Tasks;
using PremierLeagueAPI.Core;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Repositories;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Services
{
    public class SquadService : ISquadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISquadRepository _squadRepository;

        public SquadService(IUnitOfWork unitOfWork,
            ISquadRepository squadRepository)
        {
            _unitOfWork = unitOfWork;
            _squadRepository = squadRepository;
        }

        public async Task<PaginatedList<Squad>> GetAsync(SquadQuery squadQuery)
        {
            return await _squadRepository.GetAsync(squadQuery);
        }

        public async Task<Squad> GetByIdAsync(int id)
        {
            return await _squadRepository.GetAsync(id);
        }

        public async Task<Squad> GetDetailByIdAsync(int id)
        {
            return await _squadRepository.GetDetailAsync(id);
        }

        public async Task<Squad> GetDetailBySeasonIdAndClubIdAsync(int seasonId, int clubId)
        {
            return await _squadRepository.GetDetailAsync(seasonId, clubId);
        }

        public async Task CreateAsync(Squad squad)
        {
            _squadRepository.Add(squad);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Squad squad)
        {
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Squad squad)
        {
            _squadRepository.Remove(squad);
            await _unitOfWork.CompleteAsync();
        }
    }
}