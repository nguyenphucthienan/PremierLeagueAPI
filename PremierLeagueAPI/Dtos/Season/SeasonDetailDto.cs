using System;
using System.Collections.Generic;
using PremierLeagueAPI.Dtos.Squad;

namespace PremierLeagueAPI.Dtos.Season
{
    public class SeasonDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<SquadDetailDto> Squads { get; set; }
    }
}