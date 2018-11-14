using System;

namespace PremierLeagueAPI.Dtos.Match
{
    public class MatchUpdateDto
    {
        public int Round { get; set; }
        public int StadiumId { get; set; }
        public DateTime MatchTime { get; set; }
        public bool IsPlayed { get; set; }
    }
}