using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Constants;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Season;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISeasonService _seasonService;

        public SeasonsController(IMapper mapper,
            ISeasonService seasonService)
        {
            _mapper = mapper;
            _seasonService = seasonService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSeasons()
        {
            var seasons = await _seasonService.GetAllAsync();
            var returnSeasons = _mapper.Map<IEnumerable<SeasonListDto>>(seasons);

            return Ok(returnSeasons);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSeason(int id)
        {
            var season = await _seasonService.GetByIdAsync(id);

            if (season == null)
                return NotFound();

            var returnSeason = _mapper.Map<SeasonDetailDto>(season);
            return Ok(returnSeason);
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> CreateSeason([FromBody] SeasonCreateDto seasonCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var seasonToCreate = _mapper.Map<Season>(seasonCreateDto);

            await _seasonService.CreateAsync(seasonToCreate);

            var season = await _seasonService.GetByIdAsync(seasonToCreate.Id);
            var returnSeason = _mapper.Map<SeasonDetailDto>(season);

            return Ok(returnSeason);
        }

        [HttpPut("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdateSeason(int id, [FromBody] SeasonUpdateDto seasonUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var season = await _seasonService.GetByIdAsync(id);

            if (season == null)
                return NotFound();

            _mapper.Map(seasonUpdateDto, season);
            await _seasonService.UpdateAsync(season);

            var updatedSeason = await _seasonService.GetByIdAsync(id);
            var returnSeason = _mapper.Map<SeasonDetailDto>(updatedSeason);

            return Ok(returnSeason);
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> DeleteSeason(int id)
        {
            var season = await _seasonService.GetByIdAsync(id);

            if (season == null)
                return NotFound();

            await _seasonService.DeleteAsync(season);

            return Ok(id);
        }
    }
}