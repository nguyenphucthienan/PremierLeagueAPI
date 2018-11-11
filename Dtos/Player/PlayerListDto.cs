using System;

namespace PremierLeagueAPI.Dtos.Player
{
    public class PlayerListDto
    {
        public int Id { get; set; }
        public int ClubName { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string Position { get; set; }
        public string Nationality { get; set; }
        public DateTime Birthdate { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public string PhotoUrl { get; set; }
    }
}