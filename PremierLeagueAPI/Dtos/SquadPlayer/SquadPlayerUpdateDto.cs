using System;

namespace PremierLeagueAPI.Dtos.SquadPlayer
{
    public class SquadPlayerUpdateDto
    {
        public int PlayerId { get; set; }
        public int Number { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}