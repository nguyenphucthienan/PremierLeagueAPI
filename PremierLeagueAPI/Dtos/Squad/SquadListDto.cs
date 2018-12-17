using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Dtos.Season;

namespace PremierLeagueAPI.Dtos.Squad
{
    public class SquadListDto
    {
        public int Id { get; set; }
        public ClubBriefListDto Club{ get; set; }
        public SeasonBriefListDto Season { get; set; }
    }
}