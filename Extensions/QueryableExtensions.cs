using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PremierLeagueAPI.Core.Queries;

namespace PremierLeagueAPI.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> query,
            IQueryObject queryObject,
            Dictionary<string, Expression<Func<T, object>>> columnsMap)
        {
            if (String.IsNullOrWhiteSpace(queryObject.SortBy)
                || !columnsMap.ContainsKey(queryObject.SortBy))
                return query;

            return queryObject.IsSortAscending
                ? query.OrderBy(columnsMap[queryObject.SortBy])
                : query.OrderByDescending(columnsMap[queryObject.SortBy]);
        }
    }
}