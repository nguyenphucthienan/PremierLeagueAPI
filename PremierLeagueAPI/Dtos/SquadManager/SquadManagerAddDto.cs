using System;

namespace PremierLeagueAPI.Dtos.SquadManager
{
    public class SquadManagerAddDto
    {
        public int ManagerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}