using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PremierLeagueAPI.Core.Models
{
    public class Match
    {
        public Match()
        {
            Goals = new Collection<Goal>();
            Cards = new Collection<Card>();
        }

        public int Id { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; }
        public int Round { get; set; }
        public int StadiumId { get; set; }
        public Stadium Stadium { get; set; }
        public int HomeClubId { get; set; }
        public Club HomeClub { get; set; }
        public int AwayClubId { get; set; }
        public Club AwayClub { get; set; }
        public int HomeClubKitId { get; set; }
        public Kit HomeClubKit { get; set; }
        public int AwayClubKitId { get; set; }
        public Kit AwayClubKit { get; set; }

        public int HomeScore
        {
            get { return Goals.Count(g => g.ClubId == HomeClubId); }
        }

        public int AwayScore
        {
            get { return Goals.Count(g => g.ClubId == AwayClubId); }
        }

        public DateTime MatchTime { get; set; }
        public bool IsPlayed { get; set; }
        public ICollection<Goal> Goals { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}