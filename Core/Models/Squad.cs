using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PremierLeagueAPI.Core.Models
{
    public class Squad
    {
        public Squad()
        {
            Kits = new Collection<Kit>();
            SquadPlayers = new Collection<SquadPlayer>();
        }

        public int Id { get; set; }
        public int ClubId { get; set; }
        public Club Club { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; }
        public ICollection<Kit> Kits { get; set; }
        public ICollection<SquadPlayer> SquadPlayers { get; set; }
    }
}