using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PremierLeagueAPI.Core.Models
{
    public class Player
    {
        public Player()
        {
            SquadPlayers = new Collection<SquadPlayer>();
            Goals = new Collection<Goal>();
            Cards = new Collection<Card>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Nationality { get; set; }
        public DateTime Birthdate { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<SquadPlayer> SquadPlayers { get; set; }
        public ICollection<Goal> Goals { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}