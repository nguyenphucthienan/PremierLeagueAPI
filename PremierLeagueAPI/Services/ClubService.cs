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
    public class ClubService : IClubService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClubRepository _clubRepository;

        public ClubService(IUnitOfWork unitOfWork, IClubRepository clubRepository)
        {
            _unitOfWork = unitOfWork;
            _clubRepository = clubRepository;
        }

        public async Task<PaginatedList<Club>> GetAsync(ClubQuery clubQuery)
        {
            return await _clubRepository.GetAsync(clubQuery);
        }

        public async Task<IEnumerable<Club>> GetBriefListAsync(int? seasonId)
        {
            return await _clubRepository.GetBriefListAsync(seasonId);
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await _clubRepository.GetAsync(id);
        }

        public async Task<Club> GetDetailByIdAsync(int id)
        {
            return await _clubRepository.GetDetailByIdAsync(id);
        }

        public async Task CreateAsync(Club club)
        {
            _clubRepository.Add(club);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Club club)
        {
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Club club)
        {
            _clubRepository.Remove(club);
            await _unitOfWork.CompleteAsync();
        }
    }
}