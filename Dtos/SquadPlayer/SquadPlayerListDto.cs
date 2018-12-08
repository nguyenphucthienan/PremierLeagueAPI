using System;
using PremierLeagueAPI.Dtos.Player;

namespace PremierLeagueAPI.Dtos.SquadPlayer
{
    public class SquadPlayerListDto
    {
        public int SquadId { get; set; }
        public PlayerListDto Player { get; set; }
        public int Number { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}