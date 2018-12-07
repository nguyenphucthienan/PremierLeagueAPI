using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Constants;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Manager;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IManagerService _managerService;

        public ManagersController(IMapper mapper,
            IManagerService managerService)
        {
            _mapper = mapper;
            _managerService = managerService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetManagers([FromQuery] ManagerQuery managerQuery)
        {
            var managers = await _managerService.GetAsync(managerQuery);
            if (managerQuery.SquadId.HasValue)
            {
                var returnManagers = _mapper.Map<PaginatedList<ManagerListDto>>(managers,
                    opt => { opt.Items["squadId"] = managerQuery.SquadId; });

                return Ok(returnManagers);
            }
            else
            {
                var returnManagers = _mapper.Map<PaginatedList<ManagerListDto>>(managers);
                return Ok(returnManagers);
            }
        }

        [HttpGet("brief-list")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBriefListManagers()
        {
            var managers = await _managerService.GetBriefListAsync();
            var returnManagers = _mapper.Map<IEnumerable<ManagerBriefListDto>>(managers);

            return Ok(returnManagers);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetManager(int id)
        {
            var manager = await _managerService.GetDetailByIdAsync(id);

            if (manager == null)
                return NotFound();

            var returnManager = _mapper.Map<ManagerDetail>(manager);
            return Ok(returnManager);
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> CreateManager([FromBody] ManagerCreateDto managerCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var managerToCreate = _mapper.Map<Manager>(managerCreateDto);

            await _managerService.CreateAsync(managerToCreate);

            var manager = await _managerService.GetDetailByIdAsync(managerToCreate.Id);
            var returnManager = _mapper.Map<ManagerDetail>(manager);

            return Ok(returnManager);
        }

        [HttpPut("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdateManager(int id, [FromBody] ManagerUpdateDto managerUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var manager = await _managerService.GetByIdAsync(id);

            if (manager == null)
                return NotFound();

            _mapper.Map(managerUpdateDto, manager);
            await _managerService.UpdateAsync(manager);

            var updatedManager = await _managerService.GetDetailByIdAsync(id);
            var returnManager = _mapper.Map<ManagerDetail>(updatedManager);

            return Ok(returnManager);
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> DeleteManager(int id)
        {
            var manager = await _managerService.GetByIdAsync(id);

            if (manager == null)
                return NotFound();

            await _managerService.DeleteAsync(manager);

            return Ok(id);
        }
    }
}