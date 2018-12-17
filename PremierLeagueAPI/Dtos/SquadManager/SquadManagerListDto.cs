using System;
using PremierLeagueAPI.Dtos.Manager;
using PremierLeagueAPI.Dtos.Squad;

namespace PremierLeagueAPI.Dtos.SquadManager
{
    public class SquadManagerListDto
    {
        public SquadListDto Squad { get; set; }
        public ManagerListDto Manager { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}