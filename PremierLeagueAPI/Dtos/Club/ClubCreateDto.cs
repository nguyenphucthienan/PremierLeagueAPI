﻿namespace PremierLeagueAPI.Dtos.Club
{
    public class ClubCreateDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int EstablishedYear { get; set; }
        public string PhotoUrl { get; set; }
        public int StadiumId { get; set; }
    }
}