using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Dtos.Kit
{
    public class KitListDto
    {
        public int Id { get; set; }
        public KitType KitType { get; set; }
        public string PhotoUrl { get; set; }
        public int SquadId { get; set; }
    }
}