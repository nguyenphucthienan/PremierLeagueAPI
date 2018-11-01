using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Models
{
    public class Card
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; }
        public int ClubId { get; set; }
        public Club Club { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public CardType CardType { get; set; }
        public int CardTime { get; set; }
    }

    public enum CardType
    {
        Yellow,
        Red
    }
}