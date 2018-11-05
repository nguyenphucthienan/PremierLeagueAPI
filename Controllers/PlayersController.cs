using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/clubs/{clubId}/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetClubs(int clubId, [FromQuery] PlayerQuery playerQuery)
        {
            var players = await _playerService.GetByClubIdAsync(clubId, playerQuery);

            return Ok(players);
        }
    }
}