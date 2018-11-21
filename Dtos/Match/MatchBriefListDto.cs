using System;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Dtos.Stadium;

namespace PremierLeagueAPI.Dtos.Match
{
    public class MatchBriefListDto
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public int Round { get; set; }
        public StadiumBriefListDto Stadium { get; set; }
        public ClubBriefListDto HomeClub { get; set; }
        public ClubBriefListDto AwayClub { get; set; }
        public DateTime MatchTime { get; set; }
        public bool IsPlayed { get; set; }
    }
}