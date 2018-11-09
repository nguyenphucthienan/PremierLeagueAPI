using System;
using System.Collections.Generic;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Dtos.Goal;

namespace PremierLeagueAPI.Dtos.Match
{
    public class MatchDetailDto
    {
        public int Id { get; set; }
        public int Round { get; set; }
        public ClubDetailDto HomeClub { get; set; }
        public ClubDetailDto AwayClub { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public DateTime MatchTime { get; set; }
        public bool IsPlayed { get; set; }
        public ICollection<GoalListDto> Goals { get; set; }
    }
}