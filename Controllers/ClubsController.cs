using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;

namespace PremierLeagueAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly IClubService _clubService;

        public ClubsController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClubs([FromQuery] ClubQuery clubQuery)
        {
            var clubs = await _clubService.GetAsync(clubQuery);
            return Ok(clubs);
        }
    }
}