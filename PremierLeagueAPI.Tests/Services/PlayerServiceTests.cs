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
    public class PlayerServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IPlayerRepository> _playerRepository;
        private PlayerService _playerService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _playerRepository = new Mock<IPlayerRepository>();

            _playerService = new PlayerService(_unitOfWork.Object, _playerRepository.Object);
        }

        [Test]
        public async Task GetAsync_WhenCalled_GetPlayersFromDb()
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

            _playerRepository.Setup(p => p.GetAsync(playerQuery)).ReturnsAsync(expectedPlayers);

            var result = await _playerService.GetAsync(playerQuery);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedPlayers));
        }

        [Test]
        public async Task GetBriefListAsync_WhenCalled_GetPlayersFromDb()
        {
            const int squadId = 1;
            var expectedPlayers = new List<Player>()
            {
                new Player {Id = 1},
                new Player {Id = 2},
                new Player {Id = 3},
            };

            _playerRepository.Setup(p => p.GetBriefListAsync(squadId)).ReturnsAsync(expectedPlayers);

            var result = await _playerService.GetBriefListAsync(squadId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedPlayers));
        }

        [Test]
        public async Task GetAsync_WhenCalled_GetPlayerFromDb()
        {
            const int id = 1;
            var expectedPlayer = new Player
            {
                Id = id
            };

            _playerRepository.Setup(p => p.GetAsync(id)).ReturnsAsync(expectedPlayer);

            var result = await _playerService.GetByIdAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedPlayer));
        }

        [Test]
        public async Task GetDetailByIdAsync_WhenCalled_GetSeasonDetailFromDb()
        {
            const int id = 1;
            var expectedPlayer = new Player
            {
                Id = id
            };

            _playerRepository.Setup(p => p.GetDetailAsync(id)).ReturnsAsync(expectedPlayer);

            var result = await _playerService.GetDetailByIdAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedPlayer));
        }

        [Test]
        public async Task CreateAsync_WhenCalled_CreateNewPlayer()
        {
            var player = new Player
            {
                Id = 1
            };

            await _playerService.CreateAsync(player);

            _playerRepository.Verify(p => p.Add(player), Times.Once);
            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_WhenCalled_UpdateExistingPlayer()
        {
            var player = new Player
            {
                Id = 1
            };

            await _playerService.UpdateAsync(player);

            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_WhenCalled_DeletePlayerFromDb()
        {
            var player = new Player
            {
                Id = 1
            };

            await _playerService.DeleteAsync(player);

            _playerRepository.Verify(p => p.Remove(player), Times.Once);
            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}