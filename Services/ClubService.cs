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

        public async Task<Club> GetByIdAsync(int id)
        {
            return await _clubRepository.GetAsync(id);
        }

        public async Task<Club> CreateClub(Club club)
        {
            _clubRepository.Add(club);
            await _unitOfWork.CompleteAsync();

            return await _clubRepository.GetAsync(club.Id);
        }

        public async Task<Club> UpdateClub(Club club)
        {
            await _unitOfWork.CompleteAsync();
            return await _clubRepository.GetAsync(club.Id);
        }

        public async Task DeleteClub(Club club)
        {
            _clubRepository.Remove(club);
            await _unitOfWork.CompleteAsync();
        }
    }
}