using PremierLeagueAPI.Dtos.Stadium;

namespace PremierLeagueAPI.Dtos.Club
{
    public class ClubListDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int EstablishedYear { get; set; }
        public string PhotoUrl { get; set; }
        public StadiumBriefListDto Stadium { get; set; }
    }
}