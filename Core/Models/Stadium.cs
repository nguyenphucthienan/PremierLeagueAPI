using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PremierLeagueAPI.Core.Models
{
    public class Stadium
    {
        public Stadium()
        {
            Clubs = new Collection<Club>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int BuiltYear { get; set; }
        public string PitchSize { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public string MapPhotoUrl { get; set; }
        public ICollection<Club> Clubs { get; set; }
    }
}