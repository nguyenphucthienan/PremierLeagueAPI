using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Models
{
    public class Player
    {
        public Player()
        {
            Goals = new Collection<Goal>();
            Cards = new Collection<Card>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string Position { get; set; }
        public string Nationality { get; set; }
        public DateTime Birthdate { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int ClubId { get; set; }
        public Club Club { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<Goal> Goals { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}