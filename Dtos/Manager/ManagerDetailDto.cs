using System;
using System.Collections.Generic;
using PremierLeagueAPI.Dtos.SquadManager;

namespace PremierLeagueAPI.Dtos.Manager
{
    public class ManagerDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public DateTime Birthdate { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public IEnumerable<SquadManagerListDto> SquadManagers { get; set; }
    }
}