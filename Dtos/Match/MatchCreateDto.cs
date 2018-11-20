using System;

namespace PremierLeagueAPI.Dtos.Match
{
    public class MatchCreateDto
    {
        public int SeasonId { get; set; }
        public int Round { get; set; }
        public int StadiumId { get; set; }
        public int HomeClubId { get; set; }
        public int AwayClubId { get; set; }
        public DateTime MatchTime { get; set; }
        public bool IsPlayed { get; set; }
    }
}