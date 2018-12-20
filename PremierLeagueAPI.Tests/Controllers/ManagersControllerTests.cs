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
using PremierLeagueAPI.Dtos.Manager;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Tests.Controllers
{
    [TestFixture]
    public class ManagersControllerTests
    {
        private Mock<IManagerService> _managerService;
        private ManagersController _managersController;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
            var mapper = mapperConfig.CreateMapper();

            _managerService = new Mock<IManagerService>();
            _managersController = new ManagersController(mapper, _managerService.Object);
        }

        [Test]
        public async Task GetManagers_WhenCalled_ReturnManagersFromDb()
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

            _managerService.Setup(m => m.GetAsync(managerQuery)).ReturnsAsync(expectedManagers);

            var result = await _managersController.GetManagers(managerQuery);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as PaginatedList<ManagerListDto>;

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            Assert.That(okObjectResultValue, Is.Not.Null);
            Assert.That(okObjectResultValue.Pagination.PageNumber, Is.EqualTo(1));
            Assert.That(okObjectResultValue.Pagination.PageSize, Is.EqualTo(10));
            Assert.That(okObjectResultValue.Items.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetManager_WhenCalled_ReturnManagerFromDb()
        {
            const int id = 1;
            var expectedManager = new Manager
            {
                Id = id,
            };

            _managerService.Setup(p => p.GetDetailByIdAsync(id)).ReturnsAsync(expectedManager);

            var result = await _managersController.GetManager(id);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as ManagerDetailDto;

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            Assert.That(okObjectResultValue, Is.Not.Null);
            Assert.That(okObjectResultValue.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task GetManager_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            _managerService.Setup(m => m.GetDetailByIdAsync(id)).ReturnsAsync((Manager) null);

            var result = await _managersController.GetManager(id);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateManager_WhenCalled_CreateNewManager()
        {
            const int id = 1;
            const string name = "Josep Guardiola";

            var managerCreateDto = new ManagerCreateDto
            {
                Name = name
            };

            var expectedManager = new Manager
            {
                Id = id,
                Name = name
            };

            _managerService.Setup(m => m.GetDetailByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedManager);

            var result = await _managersController.CreateManager(managerCreateDto);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as ManagerDetailDto;

            _managerService.Verify(m => m.CreateAsync(It.IsAny<Manager>()), Times.Once);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(okObjectResultValue.Name, Is.EqualTo(name));
        }

        [Test]
        public async Task UpdateManager_WhenCalled_UpdateExistingManager()
        {
            const int id = 1;
            const string updateName = "Unai Emery";

            var managerUpdateDto = new ManagerUpdateDto
            {
                Name = updateName
            };

            var manager = new Manager
            {
                Id = id,
                Name = "Josep Guardiola"
            };

            var expectedManager = new Manager
            {
                Id = id,
                Name = updateName
            };

            _managerService.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(manager);
            _managerService.Setup(m => m.GetDetailByIdAsync(id)).ReturnsAsync(expectedManager);

            var result = await _managersController.UpdateManager(id, managerUpdateDto);
            var okObjectResult = result as OkObjectResult;
            var okObjectResultValue = okObjectResult.Value as ManagerDetailDto;

            _managerService.Verify(m => m.UpdateAsync(It.IsAny<Manager>()), Times.Once);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(okObjectResultValue.Name, Is.EqualTo(updateName));
        }

        [Test]
        public async Task UpdateManager_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            const string updateName = "Alexandre Lacazette";

            var managerUpdateDto = new ManagerUpdateDto
            {
                Name = updateName
            };

            _managerService.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Manager) null);

            var result = await _managersController.UpdateManager(id, managerUpdateDto);

            _managerService.Verify(m => m.GetByIdAsync(id), Times.Once);
            _managerService.Verify(m => m.UpdateAsync(It.IsAny<Manager>()), Times.Never);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteManager_WhenCalled_DeleteManagerFromDb()
        {
            const int id = 1;
            var expectedManager = new Manager
            {
                Id = id
            };

            _managerService.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(expectedManager);

            var result = await _managersController.DeleteManager(id);
            var okObjectResult = result as OkObjectResult;

            _managerService.Verify(m => m.DeleteAsync(It.IsAny<Manager>()), Times.Once);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.EqualTo(id));
        }

        [Test]
        public async Task DeleteManager_WhenCalled_ReturnNotFound()
        {
            const int id = 1;
            _managerService.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Manager) null);

            var result = await _managersController.DeleteManager(id);

            _managerService.Verify(m => m.DeleteAsync(It.IsAny<Manager>()), Times.Never);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}