using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
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