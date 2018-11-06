using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Constants;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Match;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMatchService _matchService;

        public MatchesController(IMapper mapper,
            IMatchService matchService)
        {
            _mapper = mapper;
            _matchService = matchService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMatches([FromQuery] MatchQuery matchQuery)
        {
            var matches = await _matchService.GetAsync(matchQuery);
            var returnMatches = _mapper.Map<PaginatedList<MatchListDto>>(matches);

            return Ok(returnMatches);
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> GenerateMatches()
        {
            await _matchService.GenerateMatchesAsync();
            return Ok();
        }
    }
}