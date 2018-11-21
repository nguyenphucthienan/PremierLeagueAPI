using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Dtos.Player;

namespace PremierLeagueAPI.Dtos.Goal
{
    public class GoalListDto
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public ClubBriefListDto Club { get; set; }
        public PlayerBriefListDto Player { get; set; }
        public GoalType GoalType { get; set; }
        public int GoalTime { get; set; }
        public bool IsOwnGoal { get; set; }
    }
}