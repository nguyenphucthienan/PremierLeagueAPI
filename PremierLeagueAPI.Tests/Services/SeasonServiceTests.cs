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
    public class SeasonServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ISeasonRepository> _seasonRepository;
        private SeasonService _seasonService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _seasonRepository = new Mock<ISeasonRepository>();

            _seasonService = new SeasonService(_unitOfWork.Object, _seasonRepository.Object);
        }

        [Test]
        public async Task GetAsync_WhenCalled_GetSeasonsFromDb()
        {
            var seasonQuery = new SeasonQuery
            {
                PageNumber = 1,
                PageSize = 10,
                SortBy = "name",
                IsSortAscending = true
            };

            var expectedSeasons = new PaginatedList<Season>
            {
                Pagination = new Pagination
                {
                    PageNumber = 1,
                    PageSize = 10
                },
                Items = new List<Season>()
                {
                    new Season {Id = 1},
                    new Season {Id = 2},
                    new Season {Id = 3},
                }
            };

            _seasonRepository.Setup(s => s.GetAsync(seasonQuery)).ReturnsAsync(expectedSeasons);

            var result = await _seasonService.GetAsync(seasonQuery);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedSeasons));
        }

        [Test]
        public async Task GetBriefListAsync_WhenCalled_GetSeasonsFromDb()
        {
            var expectedSeasons = new List<Season>()
            {
                new Season {Id = 1},
                new Season {Id = 2},
                new Season {Id = 3},
            };

            _seasonRepository.Setup(s => s.GetBriefListAsync()).ReturnsAsync(expectedSeasons);

            var result = await _seasonService.GetBriefListAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedSeasons));
        }

        [Test]
        public async Task GetByIdAsync_WhenCalled_GetSeasonFromDb()
        {
            const int id = 1;
            var expectedSeason = new Season
            {
                Id = id
            };

            _seasonRepository.Setup(s => s.GetAsync(id)).ReturnsAsync(expectedSeason);

            var result = await _seasonService.GetByIdAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedSeason));
        }

        [Test]
        public async Task GetDetailByIdAsync_WhenCalled_GetSeasonDetailFromDb()
        {
            const int id = 1;
            var expectedSeason = new Season
            {
                Id = id
            };

            _seasonRepository.Setup(s => s.GetDetailAsync(id)).ReturnsAsync(expectedSeason);

            var result = await _seasonService.GetDetailByIdAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedSeason));
        }

        [Test]
        public async Task CreateAsync_WhenCalled_CreateNewSeason()
        {
            var season = new Season
            {
                Id = 1
            };

            await _seasonService.CreateAsync(season);

            _seasonRepository.Verify(s => s.Add(season), Times.Once);
            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_WhenCalled_UpdateExistingSeason()
        {
            var season = new Season
            {
                Id = 1
            };

            await _seasonService.UpdateAsync(season);

            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_WhenCalled_DeleteSeasonFromDb()
        {
            var season = new Season
            {
                Id = 1
            };

            await _seasonService.DeleteAsync(season);

            _seasonRepository.Verify(s => s.Remove(season), Times.Once);
            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}