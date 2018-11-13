using System.Collections.Generic;
using PremierLeagueAPI.Dtos.Kit;

namespace PremierLeagueAPI.Dtos.Squad
{
    public class SquadDetailDto
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public int SeasonId { get; set; }
        public string SeasonName { get; set; }
        public IEnumerable<KitListDto> Kits { get; set; }
    }
}