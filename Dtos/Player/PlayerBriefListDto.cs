using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Dtos.Player
{
    public class PlayerBriefListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PositionType PositionType { get; set; }
    }
}