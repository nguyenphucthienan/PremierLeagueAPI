using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Constants;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Goal;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/matches/{matchId}/[controller]")]
    [ApiController]
    public class GoalsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlayerService _playerService;
        private readonly IMatchService _matchService;
        private readonly IGoalService _goalService;

        public GoalsController(IMapper mapper,
            IPlayerService playerService,
            IMatchService matchService,
            IGoalService goalService)
        {
            _mapper = mapper;
            _playerService = playerService;
            _matchService = matchService;
            _goalService = goalService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetGoals(int matchId)
        {
            var goals = await _goalService.GetByMatchIdAsync(matchId);
            var returnGoals = _mapper.Map<IEnumerable<GoalListDto>>(goals);

            return Ok(returnGoals);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGoal(int matchId, int id)
        {
            var goal = await _goalService.GetDetailByIdAsync(id);

            if (goal == null || goal.MatchId != matchId)
                return NotFound();

            var returnGoal = _mapper.Map<GoalDetailDto>(goal);
            return Ok(returnGoal);
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> CreateGoal(int matchId, [FromBody] GoalCreateDto goalCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var match = await _matchService.GetByIdAsync(matchId);
            var player = await _playerService.GetByIdAsync(goalCreateDto.PlayerId);

            if (match == null || player == null)
                return BadRequest();

            if (goalCreateDto.ClubId != match.HomeClubId && goalCreateDto.ClubId != match.AwayClubId)
                return BadRequest();

            if (player.ClubId != match.HomeClubId && player.ClubId != match.AwayClubId)
                return BadRequest();

            var goalToCreate = _mapper.Map<Goal>(goalCreateDto);
            goalToCreate.MatchId = matchId;

            if (player.ClubId != goalCreateDto.ClubId)
                goalToCreate.IsOwnGoal = true;

            await _goalService.CreateGoal(goalToCreate);

            var goal = await _goalService.GetDetailByIdAsync(goalToCreate.Id);
            var returnGoal = _mapper.Map<GoalDetailDto>(goal);

            return Ok(returnGoal);
        }
    }
}