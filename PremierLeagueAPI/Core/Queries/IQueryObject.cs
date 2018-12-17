namespace PremierLeagueAPI.Core.Queries
{
    public interface IQueryObject
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string SortBy { get; set; }
        bool IsSortAscending { get; set; }
    }
}