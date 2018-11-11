namespace PremierLeagueAPI.Dtos.Squad
{
    public class SquadListDto
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public int SeasonId { get; set; }
        public string SeasonName { get; set; }
    }
}