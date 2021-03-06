﻿namespace PremierLeagueAPI.Core.Queries
{
    public class GoalQuery : IQueryObject
    {
        private const int MaxPageSize = 10;

        private int _pageNumber = 1;
        private int _pageSize = MaxPageSize;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value > 0) ? value : 1;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value < MaxPageSize) ? value : MaxPageSize;
        }

        public string SortBy { get; set; } = "goalTime";
        public bool IsSortAscending { get; set; }
        public int? MatchId { get; set; }
    }
}