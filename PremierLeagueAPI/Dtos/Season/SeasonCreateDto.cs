using System;

namespace PremierLeagueAPI.Dtos.Season
{
    public class SeasonCreateDto
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
    }
}