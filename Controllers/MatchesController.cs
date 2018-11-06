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

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMatch(int id)
        {
            var match = await _matchService.GetDetailByIdAsync(id);

            if (match == null)
                return NotFound();

            var returnMatch = _mapper.Map<MatchDetailDto>(match);
            return Ok(returnMatch);
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> GenerateMatches()
        {
            await _matchService.GenerateMatchesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdateMatch(int id, [FromBody] MatchUpdateDto matchUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var match = await _matchService.GetByIdAsync(id);

            if (match == null)
                return NotFound();

            _mapper.Map(matchUpdateDto, match);
            await _matchService.UpdateMatch(match);

            var updatedMatch = await _matchService.GetDetailByIdAsync(id);
            var returnMatch = _mapper.Map<MatchDetailDto>(updatedMatch);

            return Ok(returnMatch);
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var match = await _matchService.GetByIdAsync(id);

            if (match == null)
                return BadRequest();

            await _matchService.DeleteMatchAsync(match);

            return Ok(id);
        }
    }
}