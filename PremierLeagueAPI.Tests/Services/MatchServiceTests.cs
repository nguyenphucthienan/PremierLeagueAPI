using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PremierLeagueAPI.Core;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Repositories;
using PremierLeagueAPI.Helpers;
using PremierLeagueAPI.Services;
using Match = PremierLeagueAPI.Core.Models.Match;

namespace PremierLeagueAPI.Tests.Services
{
    [TestFixture]
    public class MatchServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ISeasonRepository> _seasonRepository;
        private Mock<IClubRepository> _clubRepository;
        private Mock<ISquadRepository> _squadRepository;
        private Mock<IKitRepository> _kitRepository;
        private Mock<IMatchRepository> _matchRepository;
        private Mock<IGoalRepository> _goalRepository;
        private MatchService _matchService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _seasonRepository = new Mock<ISeasonRepository>();
            _clubRepository = new Mock<IClubRepository>();
            _squadRepository = new Mock<ISquadRepository>();
            _kitRepository = new Mock<IKitRepository>();
            _matchRepository = new Mock<IMatchRepository>();
            _goalRepository = new Mock<IGoalRepository>();

            _matchService = new MatchService(
                _unitOfWork.Object,
                _seasonRepository.Object,
                _clubRepository.Object,
                _squadRepository.Object,
                _kitRepository.Object,
                _matchRepository.Object,
                _goalRepository.Object
            );
        }

        [Test]
        public async Task GetAsync_WhenCalled_MatchesFromDb()
        {
            var matchQuery = new MatchQuery
            {
                PageNumber = 1,
                PageSize = 10,
                SortBy = "name",
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

            _matchRepository.Setup(m => m.GetAsync(matchQuery)).ReturnsAsync(expectedMatches);

            var result = await _matchService.GetAsync(matchQuery);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedMatches));
        }

        [Test]
        public async Task GetByIdAsync_WhenCalled_GetMatchFromDb()
        {
            const int id = 1;
            var expectedMatch = new Match
            {
                Id = id
            };

            _matchRepository.Setup(m => m.GetAsync(id)).ReturnsAsync(expectedMatch);

            var result = await _matchService.GetByIdAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedMatch));
        }

        [Test]
        public async Task GetDetailByIdAsync_WhenCalled_GetMatchDetailFromDb()
        {
            const int id = 1;
            var expectedMatch = new Match
            {
                Id = id
            };

            _matchRepository.Setup(m => m.GetDetailByIdAsync(id)).ReturnsAsync(expectedMatch);

            var result = await _matchService.GetDetailByIdAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedMatch));
        }

        [Test]
        public async Task GenerateAsync_WhenCalled_GenerateNewMatches()
        {
            const int seasonId = 1;

            var expectedSeason = new Season
            {
                Id = seasonId
            };

            var expectedClubs = new List<Club>()
            {
                new Club {Id = 1},
                new Club {Id = 2}
            };

            var expectedClub = new Club
            {
                Id = 1
            };

            var expectedSquad = new Squad
            {
                Id = 1
            };

            var expectedKit = new Kit
            {
                Id = 1
            };

            _seasonRepository.Setup(s => s.GetAsync(seasonId)).ReturnsAsync(expectedSeason);
            _clubRepository.Setup(c => c.GetBriefListAsync(seasonId)).ReturnsAsync(expectedClubs);
            _clubRepository.Setup(c => c.GetAsync(It.IsAny<int>())).ReturnsAsync(expectedClub);

            _squadRepository.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<Squad, bool>>>()))
                .ReturnsAsync(expectedSquad);
            _kitRepository.Setup(s => s.SingleOrDefaultAsync(It.IsAny<Expression<Func<Kit, bool>>>()))
                .ReturnsAsync(expectedKit);

            await _matchService.GenerateAsync(seasonId);

            _matchRepository.Verify(m => m.AddRange(It.Is<IEnumerable<Match>>(matches => matches.Count() == 2)),
                Times.Once);

            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAllAsync_WhenCalled_DeleteAllMatchesFromDb()
        {
            const int seasonId = 1;

            var expectedMatches = new List<Match>()
            {
                new Match {Id = 1},
                new Match {Id = 2},
                new Match {Id = 3}
            };

            _matchRepository.Setup(m => m.GetAllBySeasonIdAsync(seasonId)).ReturnsAsync(expectedMatches);

            await _matchService.DeleteAllAsync(seasonId);

            _matchRepository.Verify(m => m.RemoveRange(It.Is<IEnumerable<Match>>(matches => matches.Count() == 3)),
                Times.Once);

            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task CreateAsync_WhenCalled_CreateNewMatch()
        {
            var match = new Match
            {
                Id = 1
            };

            await _matchService.CreateAsync(match);

            _matchRepository.Verify(m => m.Add(match), Times.Once);
            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_WhenCalled_UpdateExistingMatch()
        {
            var match = new Match
            {
                Id = 1
            };

            await _matchService.UpdateAsync(match);

            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_WhenCalled_DeleteMatchFromDb()
        {
            var match = new Match
            {
                Id = 1
            };

            await _matchService.DeleteAsync(match);

            _matchRepository.Verify(m => m.Remove(match), Times.Once);
            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}