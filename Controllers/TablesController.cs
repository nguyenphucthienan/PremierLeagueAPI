using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Table;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITableService _tableService;

        public TablesController(IMapper mapper,
            ITableService tableService)
        {
            _mapper = mapper;
            _tableService = tableService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetTable(int seasonId)
        {
            var table = await _tableService.GetAsync(seasonId);
            var returnTable = _mapper.Map<IEnumerable<TableItemDto>>(table);

            return Ok(returnTable);
        }
    }
}