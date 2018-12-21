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
    public class ManagerServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IManagerRepository> _managerRepository;
        private ManagerService _managerService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _managerRepository = new Mock<IManagerRepository>();

            _managerService = new ManagerService(_unitOfWork.Object, _managerRepository.Object);
        }

        [Test]
        public async Task GetAsync_WhenCalled_GetManagersFromDb()
        {
            var managerQuery = new ManagerQuery
            {
                PageNumber = 1,
                PageSize = 10,
                SortBy = "name",
                IsSortAscending = true
            };

            var expectedManagers = new PaginatedList<Manager>
            {
                Pagination = new Pagination
                {
                    PageNumber = 1,
                    PageSize = 10
                },
                Items = new List<Manager>()
                {
                    new Manager {Id = 1},
                    new Manager {Id = 2},
                    new Manager {Id = 3},
                }
            };

            _managerRepository.Setup(m => m.GetAsync(managerQuery)).ReturnsAsync(expectedManagers);

            var result = await _managerService.GetAsync(managerQuery);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedManagers));
        }

        [Test]
        public async Task GetBriefListAsync_WhenCalled_GetManagersFromDb()
        {
            var expectedManagers = new List<Manager>()
            {
                new Manager {Id = 1},
                new Manager {Id = 2},
                new Manager {Id = 3},
            };

            _managerRepository.Setup(m => m.GetBriefListAsync()).ReturnsAsync(expectedManagers);

            var result = await _managerService.GetBriefListAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedManagers));
        }

        [Test]
        public async Task GetByIdAsync_WhenCalled_GetManagerFromDb()
        {
            const int id = 1;
            var expectedManager = new Manager
            {
                Id = id
            };

            _managerRepository.Setup(m => m.GetAsync(id)).ReturnsAsync(expectedManager);

            var result = await _managerService.GetByIdAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedManager));
        }

        [Test]
        public async Task GetDetailByIdAsync_WhenCalled_GetManagerDetailFromDb()
        {
            const int id = 1;
            var expectedManager = new Manager
            {
                Id = id
            };

            _managerRepository.Setup(m => m.GetDetailAsync(id)).ReturnsAsync(expectedManager);

            var result = await _managerService.GetDetailByIdAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedManager));
        }

        [Test]
        public async Task CreateAsync_WhenCalled_CreateNewManager()
        {
            var manager = new Manager
            {
                Id = 1
            };

            await _managerService.CreateAsync(manager);

            _managerRepository.Verify(m => m.Add(manager), Times.Once);
            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_WhenCalled_UpdateExistingManager()
        {
            var manager = new Manager
            {
                Id = 1
            };

            await _managerService.UpdateAsync(manager);

            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_WhenCalled_DeleteManagerFromDb()
        {
            var manager = new Manager
            {
                Id = 1
            };

            await _managerService.DeleteAsync(manager);

            _managerRepository.Verify(m => m.Remove(manager), Times.Once);
            _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}