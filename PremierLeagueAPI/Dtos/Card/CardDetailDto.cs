using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Dtos.Match;
using PremierLeagueAPI.Dtos.Player;

namespace PremierLeagueAPI.Dtos.Card
{
    public class CardDetailDto
    {
        public int Id { get; set; }
        public MatchBriefListDto Match { get; set; }
        public ClubBriefListDto Club { get; set; }
        public PlayerListDto Player { get; set; }
        public CardType CardType { get; set; }
        public int CardTime { get; set; }
    }
}