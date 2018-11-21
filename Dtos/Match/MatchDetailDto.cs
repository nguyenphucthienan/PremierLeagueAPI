using System;
using System.Collections.Generic;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Dtos.Goal;
using PremierLeagueAPI.Dtos.Kit;
using PremierLeagueAPI.Dtos.Season;
using PremierLeagueAPI.Dtos.Stadium;

namespace PremierLeagueAPI.Dtos.Match
{
    public class MatchDetailDto
    {
        public int Id { get; set; }
        public SeasonListDto Season { get; set; }
        public int Round { get; set; }
        public StadiumListDto Stadium { get; set; }
        public ClubListDto HomeClub { get; set; }
        public ClubListDto AwayClub { get; set; }
        public KitListDto HomeClubKit { get; set; }
        public KitListDto AwayClubKit { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public DateTime MatchTime { get; set; }
        public bool IsPlayed { get; set; }
        public ICollection<GoalListDto> Goals { get; set; }
    }
}