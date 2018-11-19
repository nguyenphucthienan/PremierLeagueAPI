using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Constants;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Kit;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/squads/{squadId}/[controller]")]
    [ApiController]
    public class KitsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISquadService _squadService;
        private readonly IKitService _kitService;

        public KitsController(IMapper mapper,
            ISquadService squadService,
            IKitService kitService)
        {
            _mapper = mapper;
            _squadService = squadService;
            _kitService = kitService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPlayers([FromQuery] KitQuery kitQuery)
        {
            var kits = await _kitService.GetAsync(kitQuery);
            var returnKits = _mapper.Map<PaginatedList<KitListDto>>(kits, opt =>
            {
                if (kitQuery.SquadId.HasValue)
                    opt.Items["squadId"] = kitQuery.SquadId;
            });

            return Ok(returnKits);
        }

        [HttpGet("list")]
        [AllowAnonymous]
        public async Task<IActionResult> GetKitsBySquadId(int squadId)
        {
            var kits = await _kitService.GetBySquadIdAsync(squadId);
            var returnKits = _mapper.Map<IEnumerable<KitListDto>>(kits);

            return Ok(returnKits);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetKit(int squadId, int id)
        {
            var squad = await _squadService.GetDetailByIdAsync(squadId);

            if (squad == null)
                return NotFound();

            var kit = await _kitService.GetByIdAsync(id);

            if (kit == null)
                return NotFound();

            if (squad.Kits.All(k => k.Id != id))
                return BadRequest();

            var returnKit = _mapper.Map<KitDetailDto>(kit);
            return Ok(returnKit);
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> CreateKit(int squadId, [FromBody] KitCreateDto kitCreateDto)
        {
            kitCreateDto.SquadId = squadId;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var kitToCreate = _mapper.Map<Kit>(kitCreateDto);

            await _kitService.CreateAsync(kitToCreate);

            var kit = await _kitService.GetDetailByIdAsync(kitToCreate.Id);
            var returnKit = _mapper.Map<KitDetailDto>(kit);

            return Ok(returnKit);
        }

        [HttpPut("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdateKit(int squadId, int id, [FromBody] KitUpdateDto kitUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var squad = await _squadService.GetDetailByIdAsync(squadId);

            if (squad == null)
                return NotFound();

            var kit = await _kitService.GetByIdAsync(id);

            if (kit == null)
                return NotFound();

            if (squad.Kits.All(k => k.Id != id))
                return BadRequest();

            _mapper.Map(kitUpdateDto, kit);
            await _kitService.UpdateAsync(kit);

            var updatedKit = await _squadService.GetByIdAsync(id);
            var returnKit = _mapper.Map<KitDetailDto>(updatedKit);

            return Ok(returnKit);
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> DeleteKit(int squadId, int id)
        {
            var squad = await _squadService.GetDetailByIdAsync(squadId);

            if (squad == null)
                return NotFound();

            var kit = await _kitService.GetByIdAsync(id);

            if (kit == null)
                return NotFound();

            if (squad.Kits.All(k => k.Id != id))
                return BadRequest();

            await _kitService.DeleteAsync(kit);

            return Ok(id);
        }
    }
}