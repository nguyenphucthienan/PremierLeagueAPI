using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Dtos.Match;
using PremierLeagueAPI.Dtos.Player;

namespace PremierLeagueAPI.Dtos.Goal
{
    public class GoalDetailDto
    {
        public int Id { get; set; }
        public MatchBriefListDto Match { get; set; }
        public ClubBriefListDto Club { get; set; }
        public PlayerListDto Player { get; set; }
        public GoalType GoalType { get; set; }
        public int GoalTime { get; set; }
        public bool IsOwnGoal { get; set; }
    }
}