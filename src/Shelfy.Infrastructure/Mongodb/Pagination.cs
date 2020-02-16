using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Shelfy.Core.Domain;
using Shelfy.Infrastructure.Pagination;

namespace Shelfy.Infrastructure.Mongodb
{
    public static class Pagination
    {
        public static async Task<PagedResult<T>> PaginateAsync<T>(this IMongoQueryable<T> collection,
            int currentPage = 1, int pageSize = 5)
        {
            if (currentPage <= 0)
                currentPage = 1;

            if (pageSize <= 0)
                pageSize = 5;

            if (pageSize > 15)
                pageSize = 15;

            var isEmpty = await collection.AnyAsync() == false;
            if (isEmpty)
                return PagedResult<T>.Empty;

            var totalResults = await collection.CountAsync();
            var totalPages = (int) Math.Ceiling((decimal) totalResults / pageSize);

            if (currentPage > totalPages)
                currentPage = totalPages;

            var paginatedSource = await collection.Limit(currentPage, pageSize).ToListAsync();

            return PagedResult<T>.Create(paginatedSource, currentPage, pageSize, totalPages, totalResults);
        }

        private static IMongoQueryable<T> Limit<T>(this IMongoQueryable<T> collection,
            int currentPage, int pageSize)
        {
            var skip = pageSize * (currentPage - 1);
            var data = collection.Skip(skip)
                .Take(pageSize);

            return data;
        }

        public static IMongoQueryable<Book> SearchQuery(this IMongoQueryable<Book> collection,
            string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return collection.OrderBy(x => x.Title);

            return collection.Where(x => x.Title.ToLowerInvariant()
                .Contains(query.ToLowerInvariant()));
        }
    }
}