﻿using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Constants;
using PremierLeagueAPI.Core.Models;
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

        [HttpPost("generate")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> GenerateMatches([FromQuery] int seasonId)
        {
            await _matchService.DeleteAllAsync(seasonId);
            await _matchService.GenerateAsync(seasonId);
            return Ok();
        }

        [HttpDelete("delete")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> DeleteMatches([FromQuery] int seasonId)
        {
            await _matchService.DeleteAllAsync(seasonId);
            return Ok();
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> CreateMatch([FromBody] MatchCreateDto matchCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var matchToCreate = _mapper.Map<Match>(matchCreateDto);
            await _matchService.CreateAsync(matchToCreate);

            var match = await _matchService.GetDetailByIdAsync(matchToCreate.Id);
            var returnMatch = _mapper.Map<MatchDetailDto>(match);

            return Ok(returnMatch);
        }

        [HttpPut("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdateMatch(int id, [FromBody] MatchUpdateDto matchUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var match = await _matchService.GetDetailByIdAsync(id);

            if (match == null)
                return NotFound();

            _mapper.Map(matchUpdateDto, match);
            await _matchService.UpdateAsync(match);

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
                return NotFound();

            await _matchService.DeleteAsync(match);

            return Ok(id);
        }

        [HttpGet("round-list")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListRounds([FromQuery] int seasonId)
        {
            var rounds = await _matchService.GetListRounds(seasonId);
            return Ok(rounds);
        }
    }
}