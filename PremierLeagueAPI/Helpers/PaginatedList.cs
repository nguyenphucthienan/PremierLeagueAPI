using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PremierLeagueAPI.Helpers
{
    public class PaginatedList<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public Pagination Pagination { get; set; }

        public PaginatedList()
        {
        }

        public PaginatedList(IEnumerable<T> items, int pageNumber, int pageSize, int totalItems)
        {
            Items = items;
            Pagination = new Pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int) Math.Ceiling(totalItems / (double) pageSize)
            };
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var totalItems = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, pageNumber, pageSize, totalItems);
        }
    }
}