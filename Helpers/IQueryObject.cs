using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Helpers
{
    public interface IQueryObject
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string SortBy { get; set; }
        bool IsSortAscending { get; set; }
    }
}