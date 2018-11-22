namespace PremierLeagueAPI.Core.Models
{
    public class TableItem
    {
        public int Rank { get; set; }
        public Club Club { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalFor { get; set; }
        public int GoalAgainst { get; set; }
        public int GoalDifference { get; set; }
        public int Point { get; set; }
    }
}