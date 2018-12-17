using System;

namespace PremierLeagueAPI.Dtos.SquadPlayer
{
    public class SquadPlayerAddDto
    {
        public int PlayerId { get; set; }
        public int Number { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}