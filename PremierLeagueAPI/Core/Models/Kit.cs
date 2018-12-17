using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PremierLeagueAPI.Core.Models
{
    public class Kit
    {
        public Kit()
        {
            HomeMatches = new Collection<Match>();
            AwayMatches = new Collection<Match>();
        }

        public int Id { get; set; }
        public KitType KitType { get; set; }
        public string PhotoUrl { get; set; }
        public int SquadId { get; set; }
        public Squad Squad { get; set; }
        public ICollection<Match> HomeMatches { get; set; }
        public ICollection<Match> AwayMatches { get; set; }
    }

    public enum KitType
    {
        HomeKit,
        AwayKit,
        ThirdKit
    }
}