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
using PremierLeagueAPI.Dtos.Player;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Tests.Controllers
{
    [TestFixture]
    public class PlayersControllerTests
    {
        private Mock<IPlayerService> _playerService;
        private PlayersController _playersController;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
            var mapper = mapperConfig.CreateMapper();

            _playerService = new Mock<IPlayerService>();
            _playersController = new PlayersController(mapper, _playerService.Object);
        }

        [Test]
        public async Task GetPlayers_WhenCalled_ReturnPlayersFromDb()
        {
            var playerQuery = new PlayerQuery
            {
                PageNumber = 1,
                PageSize = 10,
                SortBy = "name",
                IsSortAscending = true
            };

            var expectedPlayers = new PaginatedList<Player>
            {
                Pagination = new Pagination
                {
                    PageNumber = 1,
                    PageSize = 10
                },
                Items = new List<Player>()
                {
                    new Player {Id = 1},
                    new Player {Id = 2},
                    new Player {Id = 3},
                }
            };

            _playerService.Setup(p => p.GetAsync(playerQuery)).ReturnsAsync(expectedPlayers);

            var result = await _playersController.GetPlayers(playerQuery);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as PaginatedList<PlayerListDto>;

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            Assert.That(okObjectResultValue, Is.Not.Null);
            Assert.That(okObjectResultValue.Pagination.PageNumber, Is.EqualTo(1));
            Assert.That(okObjectResultValue.Pagination.PageSize, Is.EqualTo(10));
            Assert.That(okObjectResultValue.Items.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetPlayer_WhenCalled_ReturnPlayerFromDb()
        {
            const int id = 1;
            var expectedPlayer = new Player
            {
                Id = id,
            };

            _playerService.Setup(p => p.GetDetailByIdAsync(id)).ReturnsAsync(expectedPlayer);

            var result = await _playersController.GetPlayer(id);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as PlayerDetailDto;

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            Assert.That(okObjectResultValue, Is.Not.Null);
            Assert.That(okObjectResultValue.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task GetPlayer_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            _playerService.Setup(p => p.GetDetailByIdAsync(id)).ReturnsAsync((Player) null);

            var result = await _playersController.GetPlayer(id);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task CreatePlayer_WhenCalled_CreateNewPlayer()
        {
            const int id = 1;
            const string name = "Petr Cech";

            var playerCreateDto = new PlayerCreateDto
            {
                Name = name
            };

            var expectedPlayer = new Player
            {
                Id = id,
                Name = name
            };

            _playerService.Setup(p => p.GetDetailByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedPlayer);

            var result = await _playersController.CreatePlayer(playerCreateDto);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as PlayerDetailDto;

            _playerService.Verify(p => p.CreateAsync(It.IsAny<Player>()), Times.Once);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(okObjectResultValue.Name, Is.EqualTo(name));
        }

        [Test]
        public async Task UpdatePlayer_WhenCalled_UpdateExistingPlayer()
        {
            const int id = 1;
            const string updateName = "Alexandre Lacazette";

            var playerUpdateDto = new PlayerUpdateDto
            {
                Name = updateName
            };

            var player = new Player
            {
                Id = id,
                Name = "Petr Cech"
            };

            var expectedPlayer = new Player
            {
                Id = id,
                Name = updateName
            };

            _playerService.Setup(p => p.GetByIdAsync(id)).ReturnsAsync(player);
            _playerService.Setup(p => p.GetDetailByIdAsync(id)).ReturnsAsync(expectedPlayer);

            var result = await _playersController.UpdatePlayer(id, playerUpdateDto);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as PlayerDetailDto;

            _playerService.Verify(p => p.UpdateAsync(It.IsAny<Player>()), Times.Once);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(okObjectResultValue.Name, Is.EqualTo(updateName));
        }

        [Test]
        public async Task UpdatePlayer_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            const string updateName = "Alexandre Lacazette";

            var playerUpdateDto = new PlayerUpdateDto
            {
                Name = updateName
            };

            _playerService.Setup(p => p.GetByIdAsync(id)).ReturnsAsync((Player) null);

            var result = await _playersController.UpdatePlayer(id, playerUpdateDto);

            _playerService.Verify(p => p.GetByIdAsync(id), Times.Once);
            _playerService.Verify(p => p.UpdateAsync(It.IsAny<Player>()), Times.Never);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task DeletePlayer_WhenCalled_DeletePlayerFromDb()
        {
            const int id = 1;
            var expectedPlayer = new Player
            {
                Id = id
            };

            _playerService.Setup(p => p.GetByIdAsync(id)).ReturnsAsync(expectedPlayer);

            var result = await _playersController.DeletePlayer(id);
            var okObjectResult = result as OkObjectResult;

            _playerService.Verify(p => p.DeleteAsync(It.IsAny<Player>()), Times.Once);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.EqualTo(id));
        }

        [Test]
        public async Task DeletePlayer_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            _playerService.Setup(p => p.GetByIdAsync(id)).ReturnsAsync((Player) null);

            var result = await _playersController.DeletePlayer(id);

            _playerService.Verify(c => c.DeleteAsync(It.IsAny<Player>()), Times.Never);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}