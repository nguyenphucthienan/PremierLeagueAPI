namespace PremierLeagueAPI.Core.Models
{
    public class SquadPlayer
    {
        public int SquadId { get; set; }
        public Squad Squad { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int Number { get; set; }
    }
}