using System;

namespace PremierLeagueAPI.Core.Models
{
    public class SquadManager
    {
        public int SquadId { get; set; }
        public Squad Squad { get; set; }
        public int ManagerId { get; set; }
        public Manager Manager { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}