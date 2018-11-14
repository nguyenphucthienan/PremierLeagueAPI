using System;
using System.Collections.Generic;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Dtos.SquadPlayer;

namespace PremierLeagueAPI.Dtos.Player
{
    public class PlayerDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClubName { get; set; }
        public int? Number { get; set; }
        public PositionType PositionType { get; set; }
        public string Nationality { get; set; }
        public DateTime Birthdate { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public string PhotoUrl { get; set; }
        public IEnumerable<SquadPlayerListDto> SquadPlayers { get; set; }
    }
}