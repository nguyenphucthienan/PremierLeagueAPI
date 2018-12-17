using System;

namespace PremierLeagueAPI.Dtos.SquadManager
{
    public class SquadManagerUpdateDto
    {
        public int ManagerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}