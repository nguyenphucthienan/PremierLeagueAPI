using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Constants;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Player;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlayerService _playerService;

        public PlayersController(IMapper mapper,
            IPlayerService playerService)
        {
            _mapper = mapper;
            _playerService = playerService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPlayers([FromQuery] PlayerQuery playerQuery)
        {
            var players = await _playerService.GetAsync(playerQuery);
            var returnPlayers = _mapper.Map<PaginatedList<PlayerSquadListDto>>(players, opt =>
            {
                if (playerQuery.SquadId.HasValue)
                    opt.Items["squadId"] = playerQuery.SquadId;
            });

            return Ok(returnPlayers);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPlayer(int id)
        {
            var player = await _playerService.GetDetailByIdAsync(id);

            if (player == null)
                return NotFound();

            var returnPlayer = _mapper.Map<PlayerDetailDto>(player);
            return Ok(returnPlayer);
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerCreateDto playerCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var playerToCreate = _mapper.Map<Player>(playerCreateDto);

            await _playerService.CreateAsync(playerToCreate);

            var player = await _playerService.GetDetailByIdAsync(playerToCreate.Id);
            var returnPlayer = _mapper.Map<PlayerDetailDto>(player);

            return Ok(returnPlayer);
        }

        [HttpPut("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] PlayerUpdateDto playerUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var player = await _playerService.GetByIdAsync(id);

            if (player == null)
                return NotFound();

            _mapper.Map(playerUpdateDto, player);
            await _playerService.UpdateAsync(player);

            var updatedPlayer = await _playerService.GetDetailByIdAsync(id);
            var returnPlayer = _mapper.Map<PlayerDetailDto>(updatedPlayer);

            return Ok(returnPlayer);
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _playerService.GetByIdAsync(id);

            if (player == null)
                return NotFound();

            await _playerService.DeleteAsync(player);

            return Ok(id);
        }
    }
}