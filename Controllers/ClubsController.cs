using System;
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetClubs([FromQuery] ClubQuery clubQuery)
        {
            var clubs = await _clubService.GetAsync(clubQuery);
            var returnClubs = _mapper.Map<PaginatedList<ClubListDto>>(clubs);

            return Ok(returnClubs);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
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
            var club = await _clubService.CreateClub(clubToCreate);
            var returnClub = _mapper.Map<ClubDetailDto>(club);

            return Ok(returnClub);
        }
    }
}