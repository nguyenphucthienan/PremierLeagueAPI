using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Constants;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Stadium;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StadiumsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStadiumService _stadiumService;

        public StadiumsController(IMapper mapper,
            IStadiumService stadiumService)
        {
            _mapper = mapper;
            _stadiumService = stadiumService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetStadiums([FromQuery] StadiumQuery stadiumQuery)
        {
            var stadiums = await _stadiumService.GetAsync(stadiumQuery);
            var returnStadiums = _mapper.Map<PaginatedList<StadiumListDto>>(stadiums);

            return Ok(returnStadiums);
        }

        [HttpGet("brief-list")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBriefListStadiums()
        {
            var stadiums = await _stadiumService.GetBriefListAsync();
            var returnStadiums = _mapper.Map<IEnumerable<StadiumBriefListDto>>(stadiums);

            return Ok(returnStadiums);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStadium(int id)
        {
            var stadium = await _stadiumService.GetDetailByIdAsync(id);

            if (stadium == null)
                return NotFound();

            var returnStadium = _mapper.Map<StadiumDetailDto>(stadium);
            return Ok(returnStadium);
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> CreateStadium([FromBody] StadiumCreateDto stadiumCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stadiumToCreate = _mapper.Map<Stadium>(stadiumCreateDto);

            await _stadiumService.CreateAsync(stadiumToCreate);

            var stadium = await _stadiumService.GetByIdAsync(stadiumToCreate.Id);
            var returnStadium = _mapper.Map<StadiumDetailDto>(stadium);

            return Ok(returnStadium);
        }

        [HttpPut("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdateStadium(int id, [FromBody] StadiumUpdateDto stadiumUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stadium = await _stadiumService.GetByIdAsync(id);

            if (stadium == null)
                return NotFound();

            _mapper.Map(stadiumUpdateDto, stadium);
            await _stadiumService.UpdateAsync(stadium);

            var updatedStadium = await _stadiumService.GetByIdAsync(id);
            var returnStadium = _mapper.Map<StadiumDetailDto>(updatedStadium);

            return Ok(returnStadium);
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> DeleteStadium(int id)
        {
            var stadium = await _stadiumService.GetByIdAsync(id);

            if (stadium == null)
                return NotFound();

            await _stadiumService.DeleteAsync(stadium);

            return Ok(id);
        }
    }
}