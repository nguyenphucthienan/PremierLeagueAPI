using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Player;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/clubs/{clubId}/[controller]")]
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPlayers(int clubId, [FromQuery] PlayerQuery playerQuery)
        {
            var players = await _playerService.GetByClubIdAsync(clubId, playerQuery);
            var returnPlayers = _mapper.Map<PaginatedList<PlayerListDto>>(players);

            return Ok(returnPlayers);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer(int id)
        {
            var player = await _playerService.GetByIdAsync(id);

            if (player == null)
                return NotFound();

            var returnPlayer = _mapper.Map<PlayerDetailDto>(player);
            return Ok(returnPlayer);
        }
    }
}