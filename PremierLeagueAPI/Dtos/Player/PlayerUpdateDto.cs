using System;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Dtos.Player
{
    public class PlayerUpdateDto
    {
        public string Name { get; set; }
        public PositionType PositionType { get; set; }
        public string Nationality { get; set; }
        public DateTime Birthdate { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public string PhotoUrl { get; set; }
    }
}