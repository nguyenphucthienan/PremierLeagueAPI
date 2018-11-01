using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Models
{
    public class Club
    {
        public Club()
        {
            Players = new Collection<Player>();
            HomeMatches = new Collection<Match>();
            AwayMatches = new Collection<Match>();
            Goals = new Collection<Goal>();
            Cards = new Collection<Card>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int EstablishedYear { get; set; }
        public string HomeField { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<Player> Players { get; set; }
        public ICollection<Match> HomeMatches { get; set; }
        public ICollection<Match> AwayMatches { get; set; }
        public ICollection<Goal> Goals { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}