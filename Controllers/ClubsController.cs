using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IClubService _clubService;

        public ClubsController(IMapper mapper,
            IClubService clubService)
        {
            _mapper = mapper;
            _clubService = clubService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClubs([FromQuery] ClubQuery clubQuery)
        {
            var clubs = await _clubService.GetAsync(clubQuery);
            var returnClubs = _mapper.Map<PaginatedList<ClubListDto>>(clubs);

            return Ok(returnClubs);
        }
    }
}