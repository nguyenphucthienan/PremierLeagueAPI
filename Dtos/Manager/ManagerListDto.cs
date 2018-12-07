using System;

namespace PremierLeagueAPI.Dtos.Manager
{
    public class ManagerListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public DateTime Birthdate { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
    }
}