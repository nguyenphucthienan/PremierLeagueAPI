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
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Tests.Controllers
{
    [TestFixture]
    public class ClubsControllerTests
    {
        private Mock<IClubService> _clubService;
        private ClubsController _clubsController;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
            var mapper = mapperConfig.CreateMapper();

            _clubService = new Mock<IClubService>();
            _clubsController = new ClubsController(mapper, _clubService.Object);
        }

        [Test]
        public async Task GetClubs_WhenCalled_ReturnClubsFromDb()
        {
            var clubQuery = new ClubQuery
            {
                PageNumber = 1,
                PageSize = 10,
                SortBy = "name",
                IsSortAscending = true
            };

            var expectedClub = new PaginatedList<Club>
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

            _clubService.Setup(c => c.GetAsync(clubQuery)).ReturnsAsync(expectedClub);

            var result = await _clubsController.GetClubs(clubQuery);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as PaginatedList<ClubListDto>;

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            Assert.That(okObjectResultValue, Is.Not.Null);
            Assert.That(okObjectResultValue.Pagination.PageNumber, Is.EqualTo(1));
            Assert.That(okObjectResultValue.Pagination.PageSize, Is.EqualTo(10));
            Assert.That(okObjectResultValue.Items.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetBriefListClubs_WhenCalled_ReturnClubsFromDb()
        {
            const int seasonId = 1;
            var expectedClubs = new List<Club>()
            {
                new Club {Id = 1},
                new Club {Id = 2},
                new Club {Id = 3},
            };

            _clubService.Setup(c => c.GetBriefListAsync(seasonId)).ReturnsAsync(expectedClubs);

            var result = await _clubsController.GetBriefListClubs(seasonId);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as IEnumerable<ClubBriefListDto>;

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            Assert.That(okObjectResultValue, Is.Not.Null);
            Assert.That(okObjectResultValue.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetClub_WhenCalled_ReturnClubFromDb()
        {
            const int id = 1;
            var expectedClub = new Club
            {
                Id = id,
            };

            _clubService.Setup(c => c.GetDetailByIdAsync(id)).ReturnsAsync(expectedClub);

            var result = await _clubsController.GetClub(id);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as ClubDetailDto;

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            Assert.That(okObjectResultValue, Is.Not.Null);
            Assert.That(okObjectResultValue.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task GetSeason_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            _clubService.Setup(c => c.GetDetailByIdAsync(id)).ReturnsAsync((Club) null);

            var result = await _clubsController.GetClub(id);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateClub_WhenCalled_CreateNewClub()
        {
            const int id = 1;
            const string name = "Arsenal";

            var clubCreateDto = new ClubCreateDto
            {
                Name = name
            };

            var expectedClub = new Club
            {
                Id = id,
                Name = name
            };

            _clubService.Setup(c => c.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedClub);

            var result = await _clubsController.CreateClub(clubCreateDto);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as ClubDetailDto;

            _clubService.Verify(s => s.CreateAsync(It.IsAny<Club>()), Times.Once);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(okObjectResultValue.Name, Is.EqualTo(name));
        }

        [Test]
        public async Task UpdateClub_WhenCalled_UpdateExistingClub()
        {
            const int id = 1;
            const string updateName = "Chelsea";

            var clubUpdateDto = new ClubUpdateDto
            {
                Name = updateName
            };

            var club = new Club
            {
                Id = id,
                Name = "Arsenal"
            };

            var expectedClub = new Club
            {
                Id = id,
                Name = updateName
            };

            _clubService.Setup(c => c.GetByIdAsync(id)).ReturnsAsync(club);
            _clubService.Setup(c => c.GetDetailByIdAsync(id)).ReturnsAsync(expectedClub);

            var result = await _clubsController.UpdateClub(id, clubUpdateDto);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as ClubDetailDto;

            _clubService.Verify(c => c.UpdateAsync(It.IsAny<Club>()), Times.Once);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(okObjectResultValue.Name, Is.EqualTo(updateName));
        }

        [Test]
        public async Task UpdateClub_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            const string updateName = "Chelsea";

            var clubUpdateDto = new ClubUpdateDto
            {
                Name = updateName
            };

            _clubService.Setup(c => c.GetByIdAsync(id)).ReturnsAsync((Club) null);

            var result = await _clubsController.UpdateClub(id, clubUpdateDto);

            _clubService.Verify(c => c.GetByIdAsync(id), Times.Once);
            _clubService.Verify(c => c.UpdateAsync(It.IsAny<Club>()), Times.Never);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteClub_WhenCalled_DeleteClubFromDb()
        {
            const int id = 1;
            var expectedClub = new Club
            {
                Id = id
            };

            _clubService.Setup(c => c.GetByIdAsync(id)).ReturnsAsync(expectedClub);

            var result = await _clubsController.DeleteClub(id);
            var okObjectResult = result as OkObjectResult;

            _clubService.Verify(c => c.DeleteAsync(It.IsAny<Club>()), Times.Once);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.EqualTo(id));
        }

        [Test]
        public async Task DeleteClub_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            _clubService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Club) null);

            var result = await _clubsController.DeleteClub(id);

            _clubService.Verify(c => c.DeleteAsync(It.IsAny<Club>()), Times.Never);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}