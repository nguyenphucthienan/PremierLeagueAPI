using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Constants;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Controllers
{
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
        [AllowAnonymous]
        public async Task<IActionResult> GetClubs([FromQuery] ClubQuery clubQuery)
        {
            var clubs = await _clubService.GetAsync(clubQuery);
            var returnClubs = _mapper.Map<PaginatedList<ClubListDto>>(clubs);

            return Ok(returnClubs);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetClub(int id)
        {
            var club = await _clubService.GetByIdAsync(id);

            if (club == null)
                return NotFound();

            var returnClub = _mapper.Map<ClubDetailDto>(club);
            return Ok(returnClub);
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> CreateClub([FromBody] ClubCreateDto clubCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clubToCreate = _mapper.Map<Club>(clubCreateDto);
            await _clubService.CreateAsync(clubToCreate);

            var club = await _clubService.GetByIdAsync(clubToCreate.Id);
            var returnClub = _mapper.Map<ClubDetailDto>(club);

            return Ok(returnClub);
        }

        [HttpPut("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdateClub(int id, [FromBody] ClubUpdateDto clubUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var club = await _clubService.GetByIdAsync(id);

            if (club == null)
                return NotFound();

            _mapper.Map(clubUpdateDto, club);
            await _clubService.UpdateAsync(club);

            var updatedClub = await _clubService.GetByIdAsync(id);
            var returnClub = _mapper.Map<ClubDetailDto>(updatedClub);

            return Ok(returnClub);
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var club = await _clubService.GetByIdAsync(id);

            if (club == null)
                return NotFound();

            await _clubService.DeleteAsync(club);

            return Ok(id);
        }
    }
}