using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PremierLeagueAPI.Core.Models
{
    public class Manager
    {
        public Manager()
        {
            SquadManagers = new Collection<SquadManager>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public DateTime Birthdate { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<SquadManager> SquadManagers { get; set; }
    }
}