using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Constants;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Goal;
using PremierLeagueAPI.Helpers;

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
        public async Task<IActionResult> GetGoals(int matchId, [FromQuery] GoalQuery goalQuery)
        {
            goalQuery.MatchId = matchId;

            var goals = await _goalService.GetAsync(goalQuery);
            var returnGoals = _mapper.Map<PaginatedList<GoalListDto>>(goals);

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

            var goalToCreate = _mapper.Map<Goal>(goalCreateDto);
            goalToCreate.MatchId = matchId;
            // goalToCreate.IsOwnGoal = player.ClubId != goalCreateDto.ClubId;

            await _goalService.CreateAsync(goalToCreate);

            var goal = await _goalService.GetDetailByIdAsync(goalToCreate.Id);
            var returnGoal = _mapper.Map<GoalDetailDto>(goal);

            return Ok(returnGoal);
        }

        [HttpPut("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdatePlayer(int matchId, int id, [FromBody] GoalUpdateDto goalUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var goal = await _goalService.GetByIdAsync(id);
            var player = await _playerService.GetByIdAsync(goalUpdateDto.PlayerId);

            if (goal == null || player == null)
                return NotFound();

            if (goal.MatchId != matchId)
                return BadRequest();

            _mapper.Map(goalUpdateDto, goal);
            // goal.IsOwnGoal = player.ClubId != goalUpdateDto.ClubId;

            await _goalService.UpdateAsync(goal);

            var updatedGoal = await _goalService.GetDetailByIdAsync(id);
            var returnGoal = _mapper.Map<GoalDetailDto>(updatedGoal);

            return Ok(returnGoal);
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> DeleteGoal(int matchId, int id)
        {
            var goal = await _goalService.GetByIdAsync(id);

            if (goal == null)
                return NotFound();

            if (goal.MatchId != matchId)
                return BadRequest();

            await _goalService.DeleteAsync(goal);

            return Ok(id);
        }
    }
}