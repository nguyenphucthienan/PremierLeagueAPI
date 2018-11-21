using System;
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
using PremierLeagueAPI.Dtos.Player;
using PremierLeagueAPI.Dtos.Squad;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SquadsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISquadService _squadService;
        private readonly IPlayerService _playerService;

        public SquadsController(IMapper mapper,
            ISquadService squadService,
            IPlayerService playerService)
        {
            _mapper = mapper;
            _squadService = squadService;
            _playerService = playerService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSquads([FromQuery] SquadQuery squadQuery)
        {
            var squads = await _squadService.GetAsync(squadQuery);
            var returnSquads = _mapper.Map<PaginatedList<SquadListDto>>(squads);

            return Ok(returnSquads);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSquad(int id)
        {
            var squad = await _squadService.GetDetailByIdAsync(id);

            if (squad == null)
                return NotFound();

            var returnSquad = _mapper.Map<SquadDetailDto>(squad);
            return Ok(returnSquad);
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> CreateSquad([FromBody] SquadCreateDto squadCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var squadToCreate = _mapper.Map<Squad>(squadCreateDto);

            await _squadService.CreateAsync(squadToCreate);

            var squad = await _squadService.GetDetailByIdAsync(squadToCreate.Id);
            var returnSquad = _mapper.Map<SquadDetailDto>(squad);

            return Ok(returnSquad);
        }

        [HttpPut("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdateSquad(int id, [FromBody] SquadUpdateDto squadUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var squad = await _squadService.GetByIdAsync(id);

            if (squad == null)
                return NotFound();

            _mapper.Map(squadUpdateDto, squad);
            await _squadService.UpdateAsync(squad);

            var updatedSquad = await _squadService.GetByIdAsync(id);
            var returnSquad = _mapper.Map<SquadDetailDto>(updatedSquad);

            return Ok(returnSquad);
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> DeleteSquad(int id)
        {
            var squad = await _squadService.GetByIdAsync(id);

            if (squad == null)
                return NotFound();

            await _squadService.DeleteAsync(squad);

            return Ok(id);
        }

        [HttpGet("{id}/players")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPlayersInSquad(int id,
            int? seasonId, int? clubId)
        {
            var squadId = id;
            if (seasonId.HasValue && clubId.HasValue)
            {
                var squad = await _squadService.GetDetailBySeasonIdAndClubIdAsync(seasonId.Value, clubId.Value);
                squadId = squad.Id;
            }

            var players = await _playerService.GetBriefListAsync(squadId);
            var returnPlayers = _mapper.Map<IEnumerable<PlayerBriefListDto>>(players);

            return Ok(returnPlayers);
        }

        [HttpPost("{id}/players")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> AddPlayerToSquad(int id,
            [FromBody] SquadAddPlayerDto squadAddPlayerDto)
        {
            var squad = await _squadService.GetByIdAsync(id);

            if (squad == null)
                return NotFound();

            var player = await _playerService.GetByIdAsync(squadAddPlayerDto.PlayerId);

            if (player == null)
                return NotFound();

            squad.SquadPlayers.Add(new SquadPlayer
            {
                Squad = squad,
                Player = player,
                Number = squadAddPlayerDto.Number,
                StartDate = DateTime.Now
            });

            await _squadService.UpdateAsync(squad);
            return Ok();
        }

        [HttpPut("{id}/players/{playerId}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdatePlayerInSquad(int id, int playerId,
            [FromBody] SquadUpdatePlayerDto squadUpdatePlayerDto)
        {
            var squad = await _squadService.GetDetailByIdAsync(id);

            if (squad == null)
                return NotFound();

            var player = await _playerService.GetByIdAsync(squadUpdatePlayerDto.PlayerId);

            if (player == null)
                return NotFound();

            var squadPlayer = squad.SquadPlayers.SingleOrDefault(sp => sp.PlayerId == playerId);
            var existSquadPlayer = squad.SquadPlayers.SingleOrDefault(sp => sp.Number == squadUpdatePlayerDto.Number);

            if (squadPlayer == null)
                return BadRequest();

            if (existSquadPlayer != null && (existSquadPlayer.PlayerId != squadPlayer.PlayerId))
                return BadRequest();

            _mapper.Map(squadUpdatePlayerDto, squadPlayer);

            await _squadService.UpdateAsync(squad);
            return Ok();
        }

        [HttpDelete("{id}/players/{playerId}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> RemovePlayerFromSquad(int id, int playerId)
        {
            var squad = await _squadService.GetDetailByIdAsync(id);

            if (squad == null)
                return NotFound();

            var player = squad.SquadPlayers
                .SingleOrDefault(sp => sp.PlayerId == playerId);

            if (player == null)
                return NotFound();

            squad.SquadPlayers.Remove(player);
            await _squadService.UpdateAsync(squad);

            return Ok();
        }
    }
}