using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PremierLeagueAPI.Controllers;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Season;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Tests.Controllers
{
    [TestFixture]
    public class SeasonsControllerTests
    {
        private Mock<ISeasonService> _seasonService;
        private SeasonsController _seasonsController;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
            var mapper = mapperConfig.CreateMapper();

            _seasonService = new Mock<ISeasonService>();
            _seasonsController = new SeasonsController(mapper, _seasonService.Object);
        }

        [Test]
        public async Task GetSeasons_WhenCalled_ReturnSeasonsFromDb()
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

            _seasonService.Setup(s => s.GetAsync(seasonQuery)).ReturnsAsync(expectedSeasons);

            var result = await _seasonsController.GetSeasons(seasonQuery);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as PaginatedList<SeasonListDto>;

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);

            Assert.IsNotNull(okObjectResultValue);
            Assert.AreEqual(1, okObjectResultValue.Pagination.PageNumber);
            Assert.AreEqual(10, okObjectResultValue.Pagination.PageSize);
            Assert.AreEqual(3, okObjectResultValue.Items.Count());
        }

        [Test]
        public async Task GetBriefListSeasons_WhenCalled_ReturnSeasonsFromDb()
        {
            var expectedSeasons = new List<Season>()
            {
                new Season {Id = 1},
                new Season {Id = 2},
                new Season {Id = 3},
            };

            _seasonService.Setup(s => s.GetBriefListAsync()).ReturnsAsync(expectedSeasons);

            var result = await _seasonsController.GetBriefListSeasons();
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as IEnumerable<SeasonBriefListDto>;

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);

            Assert.IsNotNull(okObjectResultValue);
            Assert.AreEqual(3, okObjectResultValue.Count());
        }

        [Test]
        public async Task GetSeason_WhenCalled_ReturnSeasonFromDb()
        {
            const int id = 1;
            var expectedSeason = new Season
            {
                Id = id,
            };

            _seasonService.Setup(s => s.GetDetailByIdAsync(id)).ReturnsAsync(expectedSeason);

            var result = await _seasonsController.GetSeason(id);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as SeasonDetailDto;

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);

            Assert.IsNotNull(okObjectResultValue);
            Assert.AreEqual(id, okObjectResultValue.Id);
        }

        [Test]
        public async Task GetSeason_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            _seasonService.Setup(s => s.GetDetailByIdAsync(id)).ReturnsAsync((Season)null);

            var result = await _seasonsController.GetSeason(id);
            var notFoundResult = result as NotFoundResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task CreateSeason_WhenCalled_CreateNewSeason()
        {
            const int id = 1;
            const string name = "2018/2019";

            var seasonCreateDto = new SeasonCreateDto
            {
                Name = name
            };

            var expectedSeason = new Season
            {
                Id = id,
                Name = name
            };

            _seasonService.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedSeason);

            var result = await _seasonsController.CreateSeason(seasonCreateDto);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as SeasonDetailDto;

            _seasonService.Verify(s => s.CreateAsync(It.IsAny<Season>()), Times.Once);

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);
            Assert.AreEqual(name, okObjectResultValue.Name);
        }

        [Test]
        public async Task UpdateSeason_WhenCalled_UpdateExistingSeason()
        {
            const int id = 1;
            const string updateName = "2019/2020";

            var seasonUpdateDto = new SeasonUpdateDto
            {
                Name = updateName
            };

            var season = new Season
            {
                Id = id,
                Name = "2018/2019"
            };

            var expectedSeason = new Season
            {
                Id = id,
                Name = updateName
            };

            _seasonService.SetupSequence(s => s.GetByIdAsync(id))
                .ReturnsAsync(season)
                .ReturnsAsync(expectedSeason);

            var result = await _seasonsController.UpdateSeason(id, seasonUpdateDto);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as SeasonDetailDto;

            _seasonService.Verify(s => s.GetByIdAsync(id), Times.Exactly(2));
            _seasonService.Verify(s => s.UpdateAsync(It.IsAny<Season>()), Times.Once);

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);
            Assert.AreEqual(updateName, okObjectResultValue.Name);
        }

        [Test]
        public async Task UpdateSeason_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            const string updateName = "2019/2020";

            var seasonUpdateDto = new SeasonUpdateDto
            {
                Name = updateName
            };

            _seasonService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Season) null);

            var result = await _seasonsController.UpdateSeason(id, seasonUpdateDto);
            var notFoundResult = result as NotFoundResult;

            _seasonService.Verify(s => s.GetByIdAsync(id), Times.Once);
            _seasonService.Verify(s => s.UpdateAsync(It.IsAny<Season>()), Times.Never);

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task DeleteSeason_WhenCalled_DeleteSeasonFromDb()
        {
            const int id = 1;
            var expectedSeason = new Season
            {
                Id = id
            };

            _seasonService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(expectedSeason);

            var result = await _seasonsController.DeleteSeason(id);
            var okObjectResult = result as OkObjectResult;

            _seasonService.Verify(s => s.DeleteAsync(It.IsAny<Season>()), Times.Once);

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);
            Assert.AreEqual(1, okObjectResult.Value);
        }

        [Test]
        public async Task DeleteSeason_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            _seasonService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Season)null);

            var result = await _seasonsController.DeleteSeason(id);
            var notFoundResult = result as NotFoundResult;

            _seasonService.Verify(s => s.DeleteAsync(It.IsAny<Season>()), Times.Never);

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
    }
}