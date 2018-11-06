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
        private readonly IGoalService _goalService;

        public GoalsController(IMapper mapper,
            IGoalService goalService)
        {
            _mapper = mapper;
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
            goalCreateDto.MatchId = matchId;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var goalToCreate = _mapper.Map<Goal>(goalCreateDto);
            await _goalService.CreateGoal(goalToCreate);

            var goal = await _goalService.GetDetailByIdAsync(goalToCreate.Id);
            var returnGoal = _mapper.Map<GoalDetailDto>(goal);

            return Ok(returnGoal);
        }
    }
}