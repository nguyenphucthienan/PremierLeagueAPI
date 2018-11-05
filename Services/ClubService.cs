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
    }
}