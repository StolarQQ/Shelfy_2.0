using System;
using System.Collections.Generic;
using System.Linq;
using Shelfy.Core.Helper;

namespace Shelfy.Infrastructure.Extensions
{
    public static class PaginationExtensions
    {
        public static PagedResult<T> Paginate<T>(this IEnumerable<T> source,
            int currentPage, int pageSize)
        {
            if (source == null || source.Any() == false)
                return PagedResult<T>.Empty;

            if (currentPage <= 0) currentPage = 1;

            if (pageSize <= 0) pageSize = 10;

            if (pageSize > 15) pageSize = 15;

            var totalCount = source.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var paginatedSource = source.Skip(pageSize * (currentPage - 1)).Take(pageSize);

            return PagedResult<T>.Create(paginatedSource, totalPages, currentPage, pageSize, totalCount);
        }
    }
}