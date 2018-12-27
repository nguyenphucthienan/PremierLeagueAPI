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
using PremierLeagueAPI.Dtos.SquadPlayer;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/squads/{squadId}/players")]
    [ApiController]
    public class SquadPlayersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISquadService _squadService;
        private readonly IPlayerService _playerService;

        public SquadPlayersController(IMapper mapper,
            ISquadService squadService,
            IPlayerService playerService)
        {
            _mapper = mapper;
            _squadService = squadService;
            _playerService = playerService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPlayersInSquad(int squadId,
            [FromQuery] SquadPlayerQuery squadPlayerQuery)
        {
            squadPlayerQuery.SquadId = squadId;
            if (squadPlayerQuery.SeasonId.HasValue && squadPlayerQuery.ClubId.HasValue)
            {
                var squad = await _squadService
                    .GetDetailBySeasonIdAndClubIdAsync(squadPlayerQuery.SeasonId.Value, squadPlayerQuery.ClubId.Value);
                squadPlayerQuery.SquadId = squad.Id;
            }

            var players = await _squadService.GetPlayersInSquadAsync(squadPlayerQuery);
            var returnPlayers = _mapper.Map<PaginatedList<SquadPlayerListDto>>(players);

            return Ok(returnPlayers);
        }

        [HttpGet("brief-list")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPlayersInSquad(int squadId, int? seasonId, int? clubId)
        {
            var squadIdValue = squadId;
            if (seasonId.HasValue && clubId.HasValue)
            {
                var squad = await _squadService.GetDetailBySeasonIdAndClubIdAsync(seasonId.Value, clubId.Value);
                squadIdValue = squad.Id;
            }

            var players = await _playerService.GetBriefListAsync(squadIdValue);
            var returnPlayers = _mapper.Map<IEnumerable<PlayerBriefListDto>>(players);

            return Ok(returnPlayers);
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> AddPlayerToSquad(int squadId,
            [FromBody] SquadPlayerAddDto squadPlayerAddDto)
        {
            var squad = await _squadService.GetDetailByIdAsync(squadId);

            if (squad == null)
                return NotFound();

            var player = await _playerService.GetDetailByIdAsync(squadPlayerAddDto.PlayerId);

            if (player == null)
                return NotFound();

            if (player.SquadPlayers.Any(sp => sp.Squad.SeasonId == squad.SeasonId))
                return BadRequest();

            if (squad.SquadPlayers.Any(sp => sp.Number == squadPlayerAddDto.Number))
                return BadRequest();

            if (squadPlayerAddDto.StartDate < player.Birthdate)
                return BadRequest();

            if (squadPlayerAddDto.EndDate.HasValue && squadPlayerAddDto.EndDate.Value < squadPlayerAddDto.StartDate)
                return BadRequest();

            squad.SquadPlayers.Add(new SquadPlayer
            {
                Squad = squad,
                Player = player,
                Number = squadPlayerAddDto.Number,
                StartDate = squadPlayerAddDto.StartDate,
                EndDate = squadPlayerAddDto.EndDate
            });

            await _squadService.UpdateAsync(squad);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdatePlayerInSquad(int squadId, int id,
            [FromBody] SquadPlayerUpdateDto squadPlayerUpdateDto)
        {
            var squad = await _squadService.GetDetailByIdAsync(squadId);

            if (squad == null)
                return NotFound();

            var player = await _playerService.GetByIdAsync(squadPlayerUpdateDto.PlayerId);

            if (player == null)
                return NotFound();

            var squadPlayer = squad.SquadPlayers.SingleOrDefault(sp => sp.PlayerId == squadPlayerUpdateDto.PlayerId);
            var existSquadPlayer = squad.SquadPlayers.SingleOrDefault(sp => sp.Number == squadPlayerUpdateDto.Number);

            if (squadPlayer == null)
                return BadRequest();

            if (existSquadPlayer != null && (existSquadPlayer.PlayerId != squadPlayer.PlayerId))
                return BadRequest();

            if (squadPlayerUpdateDto.StartDate < player.Birthdate)
                return BadRequest();

            if (squadPlayerUpdateDto.EndDate.HasValue &&
                squadPlayerUpdateDto.EndDate.Value < squadPlayerUpdateDto.StartDate)
                return BadRequest();

            _mapper.Map(squadPlayerUpdateDto, squadPlayer);

            await _squadService.UpdateAsync(squad);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> RemovePlayerFromSquad(int squadId, int id)
        {
            var squad = await _squadService.GetDetailByIdAsync(squadId);

            if (squad == null)
                return NotFound();

            var player = squad.SquadPlayers
                .SingleOrDefault(sp => sp.PlayerId == id);

            if (player == null)
                return NotFound();

            squad.SquadPlayers.Remove(player);
            await _squadService.UpdateAsync(squad);

            return Ok();
        }
    }
}