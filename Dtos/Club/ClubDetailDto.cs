﻿using System.Collections.Generic;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Dtos.Club
{
    public class ClubDetailDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int EstablishedYear { get; set; }
        public string HomeField { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}