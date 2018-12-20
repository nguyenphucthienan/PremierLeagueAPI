using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PremierLeagueAPI.Core;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Repositories;
using PremierLeagueAPI.Helpers;
using PremierLeagueAPI.Services;

namespace PremierLeagueAPI.Tests.Services
{
    [TestFixture]
    public class ClubServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IClubRepository> _clubRepository;
        private ClubService _clubService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _clubRepository = new Mock<IClubRepository>();

            _clubService = new ClubService(_unitOfWork.Object, _clubRepository.Object);
        }

        [Test]
        public async Task GetAsync_WhenCalled_ClubsFromDb()
        {
            var clubQuery = new ClubQuery
            {
                PageNumber = 1,
                PageSize = 10,
                SortBy = "name",
                IsSortAscending = true
            };

            var expectedClubs = new PaginatedList<Club>
            {
                Pagination = new Pagination
                {
                    PageNumber = 1,
                    PageSize = 10
                },
                Items = new List<Club>()
                {
                    new Club {Id = 1},
                    new Club {Id = 2},
                    new Club {Id = 3},
                }
            };

            _clubRepository.Setup(c => c.GetAsync(clubQuery)).ReturnsAsync(expectedClubs);

            var result = await _clubService.GetAsync(clubQuery);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedClubs));
        }

        [Test]
        public async Task GetBriefListAsync_WhenCalled_GetBriefListClubsFromDb()
        {
            const int seasonId = 1;
            var expectedClubs = new List<Club>()
            {
                new Club {Id = 1},
                new Club {Id = 2},
                new Club {Id = 3},
            };

            _clubRepository.Setup(c => c.GetBriefListAsync(seasonId)).ReturnsAsync(expectedClubs);

            var result = await _clubService.GetBriefListAsync(seasonId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedClubs));
        }

        [Test]
        public async Task GetAsync_WhenCalled_GetClubFromDb()
        {
            const int id = 1;
            var expectedClub = new Club
            {
                Id = id
            };

            _clubRepository.Setup(c => c.GetAsync(id)).ReturnsAsync(expectedClub);

            var result = await _clubService.GetByIdAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedClub));
        }

        [Test]
        public async Task GetDetailByIdAsync_WhenCalled_GetClubFromDb()
        {
            const int id = 1;
            var expectedClub = new Club
            {
                Id = id
            };

            _clubRepository.Setup(c => c.GetDetailByIdAsync(id)).ReturnsAsync(expectedClub);

            var result = await _clubService.GetDetailByIdAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedClub));
        }

        [Test]
        public async Task CreateAsync_WhenCalled_CreateNewClub()
        {
            var club = new Club
            {
                Id = 1
            };

            await _clubService.CreateAsync(club);

            _clubRepository.Verify(c => c.Add(club), Times.Once);
            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_WhenCalled_UpdateExistingClub()
        {
            var club = new Club
            {
                Id = 1
            };

            await _clubService.UpdateAsync(club);

            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_WhenCalled_DeleteSeasonFromDb()
        {
            var club = new Club
            {
                Id = 1
            };

            await _clubService.DeleteAsync(club);

            _clubRepository.Verify(c => c.Remove(club), Times.Once);
            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}