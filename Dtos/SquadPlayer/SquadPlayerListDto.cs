﻿using System;
using PremierLeagueAPI.Dtos.Squad;

namespace PremierLeagueAPI.Dtos.SquadPlayer
{
    public class SquadPlayerListDto
    {
        public SquadListDto Squad { get; set; }
        public int Number { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}