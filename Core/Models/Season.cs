using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PremierLeagueAPI.Core.Models
{
    public class Season
    {
        public Season()
        {
            SeasonClubs = new Collection<SeasonClub>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<SeasonClub> SeasonClubs { get; set; }
    }
}