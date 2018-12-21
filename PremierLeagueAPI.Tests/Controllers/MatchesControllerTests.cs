using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PremierLeagueAPI.Controllers;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Match;
using PremierLeagueAPI.Helpers;
using Match = PremierLeagueAPI.Core.Models.Match;

namespace PremierLeagueAPI.Tests.Controllers
{
    [TestFixture]
    public class MatchesControllerTests
    {
        private Mock<IMatchService> _matchService;
        private MatchesController _matchesController;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
            var mapper = mapperConfig.CreateMapper();

            _matchService = new Mock<IMatchService>();
            _matchesController = new MatchesController(mapper, _matchService.Object);
        }

        [Test]
        public async Task GetMatches_WhenCalled_ReturnMatchesFromDb()
        {
            var matchQuery = new MatchQuery
            {
                PageNumber = 1,
                PageSize = 10,
                SortBy = "id",
                IsSortAscending = true
            };

            var expectedMatches = new PaginatedList<Match>
            {
                Pagination = new Pagination
                {
                    PageNumber = 1,
                    PageSize = 10
                },
                Items = new List<Match>()
                {
                    new Match {Id = 1},
                    new Match {Id = 2},
                    new Match {Id = 3},
                }
            };

            _matchService.Setup(m => m.GetAsync(matchQuery)).ReturnsAsync(expectedMatches);

            var result = await _matchesController.GetMatches(matchQuery);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as PaginatedList<MatchListDto>;

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            Assert.That(okObjectResultValue, Is.Not.Null);
            Assert.That(okObjectResultValue.Pagination.PageNumber, Is.EqualTo(1));
            Assert.That(okObjectResultValue.Pagination.PageSize, Is.EqualTo(10));
            Assert.That(okObjectResultValue.Items.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetMatch_WhenCalled_ReturnMatchFromDb()
        {
            const int id = 1;
            var expectedMatch = new Match
            {
                Id = id,
            };

            _matchService.Setup(m => m.GetDetailByIdAsync(id)).ReturnsAsync(expectedMatch);

            var result = await _matchesController.GetMatch(id);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as MatchDetailDto;

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            Assert.That(okObjectResultValue, Is.Not.Null);
            Assert.That(okObjectResultValue.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task GetMatch_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            _matchService.Setup(m => m.GetDetailByIdAsync(id)).ReturnsAsync((Match) null);

            var result = await _matchesController.GetMatch(id);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task GenerateMatches_WhenCalled_GenerateNewMatches()
        {
            const int seasonId = 1;
            var result = await _matchesController.GenerateMatches(seasonId);

            _matchService.Verify(m => m.DeleteAllAsync(seasonId), Times.Once);
            _matchService.Verify(m => m.GenerateAsync(seasonId), Times.Once);

            Assert.That(result, Is.TypeOf<OkResult>());
        }

        [Test]
        public async Task DeleteMatches_WhenCalled_DeleteMatchesFromDb()
        {
            const int seasonId = 1;
            var result = await _matchesController.DeleteMatches(seasonId);

            _matchService.Verify(m => m.DeleteAllAsync(seasonId), Times.Once);

            Assert.That(result, Is.TypeOf<OkResult>());
        }

        [Test]
        public async Task CreateMatch_WhenCalled_CreateNewMatch()
        {
            const int id = 1;

            var matchCreateDto = new MatchCreateDto
            {
                Round = 1
            };

            var expectedMatch = new Match
            {
                Id = id,
                Round = 1
            };

            _matchService.Setup(m => m.GetDetailByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedMatch);

            var result = await _matchesController.CreateMatch(matchCreateDto);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as MatchDetailDto;

            _matchService.Verify(m => m.CreateAsync(It.IsAny<Match>()), Times.Once);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(okObjectResultValue.Round, Is.EqualTo(1));
        }

        [Test]
        public async Task UpdateaMatch_WhenCalled_UpdateExistingMatch()
        {
            const int id = 1;
            const int updateRound = 2;

            var matchUpdateDto = new MatchUpdateDto
            {
                Round = updateRound
            };

            var match = new Match
            {
                Id = id,
                Round = 1
            };

            var expectedMatch = new Match
            {
                Id = id,
                Round = updateRound
            };

            _matchService.SetupSequence(m => m.GetDetailByIdAsync(id))
                .ReturnsAsync(match)
                .ReturnsAsync(expectedMatch);

            var result = await _matchesController.UpdateMatch(id, matchUpdateDto);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as MatchDetailDto;

            _matchService.Verify(m => m.GetDetailByIdAsync(id), Times.Exactly(2));
            _matchService.Verify(m => m.UpdateAsync(It.IsAny<Match>()), Times.Once);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(okObjectResultValue.Round, Is.EqualTo(updateRound));
        }

        [Test]
        public async Task UpdateMatch_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            const int updateRound = 2;

            var matchUpdateDto = new MatchUpdateDto
            {
                Round = updateRound
            };

            _matchService.Setup(m => m.GetDetailByIdAsync(id)).ReturnsAsync((Match) null);

            var result = await _matchesController.UpdateMatch(id, matchUpdateDto);

            _matchService.Verify(m => m.GetDetailByIdAsync(id), Times.Once);
            _matchService.Verify(m => m.UpdateAsync(It.IsAny<Match>()), Times.Never);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }


        [Test]
        public async Task DeleteMatch_WhenCalled_DeleteMatchFromDb()
        {
            const int id = 1;
            var expectedMatch = new Match
            {
                Id = id
            };

            _matchService.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(expectedMatch);

            var result = await _matchesController.DeleteMatch(id);
            var okObjectResult = result as OkObjectResult;

            _matchService.Verify(m => m.DeleteAsync(It.IsAny<Match>()), Times.Once);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.EqualTo(id));
        }

        [Test]
        public async Task DeleteMatch_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            _matchService.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Match) null);

            var result = await _matchesController.DeleteMatch(id);

            _matchService.Verify(m => m.DeleteAsync(It.IsAny<Match>()), Times.Never);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}