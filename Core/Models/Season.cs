using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PremierLeagueAPI.Core.Models
{
    public class Season
    {
        public Season()
        {
            Squads = new Collection<Squad>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<Squad> Squads { get; set; }
    }
}