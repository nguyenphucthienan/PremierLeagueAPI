using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Dtos.Player;

namespace PremierLeagueAPI.Dtos.Card
{
    public class CardListDto
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public ClubBriefListDto Club { get; set; }
        public PlayerBriefListDto Player { get; set; }
        public CardType CardType { get; set; }
        public int CardTime { get; set; }
    }
}