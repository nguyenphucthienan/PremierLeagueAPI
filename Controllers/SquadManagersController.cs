using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Constants;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.SquadManager;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/squads/{squadId}/managers")]
    [ApiController]
    public class SquadManagersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISquadService _squadService;
        private readonly IManagerService _managerService;

        public SquadManagersController(IMapper mapper,
            ISquadService squadService,
            IManagerService managerService)
        {
            _mapper = mapper;
            _squadService = squadService;
            _managerService = managerService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetManagersInSquad(int squadId,
            [FromQuery] SquadManagerQuery squadManagerQuery)
        {
            squadManagerQuery.SquadId = squadId;
            if (squadManagerQuery.SeasonId.HasValue && squadManagerQuery.ClubId.HasValue)
            {
                var squad = await _squadService
                    .GetDetailBySeasonIdAndClubIdAsync(squadManagerQuery.SeasonId.Value,
                        squadManagerQuery.ClubId.Value);
                squadManagerQuery.SquadId = squad.Id;
            }

            var managers = await _managerService.GetManagersInSquadAsync(squadManagerQuery);
            var returnManagers = _mapper.Map<PaginatedList<SquadManagerListDto>>(managers);

            return Ok(returnManagers);
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> AddManagerToSquad(int squadId,
            [FromBody] SquadManagerAddDto squadManagerAddDto)
        {
            var squad = await _squadService.GetDetailByIdAsync(squadId);

            if (squad == null)
                return NotFound();

            var manager = await _managerService.GetDetailByIdAsync(squadManagerAddDto.ManagerId);

            if (manager == null)
                return NotFound();

            if (manager.SquadManagers.Any(sp => sp.Squad.SeasonId == squad.SeasonId))
                return BadRequest();

            squad.SquadManagers.Add(new SquadManager
            {
                Squad = squad,
                Manager = manager,
                StartDate = DateTime.Now
            });

            await _squadService.UpdateAsync(squad);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdateManagerInSquad(int squadId, int id,
            [FromBody] SquadManagerUpdateDto squadManagerUpdateDto)
        {
            var squad = await _squadService.GetDetailByIdAsync(squadId);

            if (squad == null)
                return NotFound();

            var manager = await _managerService.GetByIdAsync(squadManagerUpdateDto.ManagerId);

            if (manager == null)
                return NotFound();

            var squadManager = squad.SquadManagers.SingleOrDefault(sm => sm.ManagerId == id);

            if (squadManager == null)
                return BadRequest();

            _mapper.Map(squadManagerUpdateDto, squadManager);

            await _squadService.UpdateAsync(squad);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> RemoveManagerFromSquad(int squadId, int id)
        {
            var squad = await _squadService.GetDetailByIdAsync(squadId);

            if (squad == null)
                return NotFound();

            var manager = squad.SquadManagers
                .SingleOrDefault(sp => sp.ManagerId == id);

            if (manager == null)
                return NotFound();

            squad.SquadManagers.Remove(manager);
            await _squadService.UpdateAsync(squad);

            return Ok();
        }
    }
}