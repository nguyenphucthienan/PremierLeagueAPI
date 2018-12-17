using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Dtos.Kit
{
    public class KitUpdateDto
    {
        public KitType KitType { get; set; }
        public string PhotoUrl { get; set; }
        public int SquadId { get; set; }
    }
}