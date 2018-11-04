using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Core.Models
{
    public class Match
    {
        public Match()
        {
            Goals = new Collection<Goal>();
            Cards = new Collection<Card>();
        }

        public int Id { get; set; }
        public int Round { get; set; }
        public int HomeClubId { get; set; }
        public Club HomeClub { get; set; }
        public int AwayClubId { get; set; }
        public Club AwayClub { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public DateTime MatchTime { get; set; }
        public bool IsPlayed { get; set; }
        public ICollection<Goal> Goals { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}