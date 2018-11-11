namespace PremierLeagueAPI.Core.Models
{
    public class SeasonClub
    {
        public int SeasonId { get; set; }
        public Season Season { get; set; }
        public int ClubId { get; set; }
        public Club Club { get; set; }
    }
}