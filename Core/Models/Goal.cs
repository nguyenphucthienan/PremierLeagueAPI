namespace PremierLeagueAPI.Core.Models
{
    public class Goal
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; }
        public int ClubId { get; set; }
        public Club Club { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public GoalType GoalType { get; set; }
        public int GoalTime { get; set; }
        public bool IsOwnGoal { get; set; }
    }

    public enum GoalType
    {
        Other,
        LeftFoot,
        RightFoot,
        Head
    }
}