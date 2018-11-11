namespace PremierLeagueAPI.Core.Queries
{
    public class PlayerQuery : IQueryObject
    {
        private const int MaxPageSize = 20;

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

        public string SortBy { get; set; } = "name";
        public bool IsSortAscending { get; set; }
        public int? SquadId { get; set; }
        public string Position { get; set; }
    }
}