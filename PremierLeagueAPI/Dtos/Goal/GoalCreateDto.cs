using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Dtos.Goal
{
    public class GoalCreateDto
    {
        public int ClubId { get; set; }
        public int PlayerId { get; set; }
        public GoalType GoalType { get; set; }
        public int GoalTime { get; set; }
    }
}