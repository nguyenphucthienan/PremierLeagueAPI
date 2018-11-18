using System.Collections.Generic;
using PremierLeagueAPI.Dtos.Squad;
using PremierLeagueAPI.Dtos.Stadium;

namespace PremierLeagueAPI.Dtos.Club
{
    public class ClubDetailDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int EstablishedYear { get; set; }
        public string PhotoUrl { get; set; }
        public StadiumListDto Stadium { get; set; }
        public IEnumerable<SquadDetailDto> Squads { get; set; }
    }
}