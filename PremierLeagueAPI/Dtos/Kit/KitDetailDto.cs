using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Dtos.Squad;

namespace PremierLeagueAPI.Dtos.Kit
{
    public class KitDetailDto
    {
        public int Id { get; set; }
        public KitType KitType { get; set; }
        public string PhotoUrl { get; set; }
        public SquadListDto Squad { get; set; }
    }
}