using System.Collections.Generic;
using System.Linq;
using Shelfy.Core.Pagination;

namespace Shelfy.Infrastructure.Pagination
{
    /// <summary>
    /// Pagination metadata
    /// </summary>
    public class PagedResult<T> : IPagedResult<T>
    {
        public IEnumerable<T> Source { get; set; }
        public int CurrentPage { get; } = 1;
        public int PageSize { get; } = 5;
        public int TotalPages { get; }
        public int TotalCount { get; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public bool ShowFirst => CurrentPage != 1;
        public bool ShowLast => CurrentPage != TotalPages;

        private PagedResult()
        {
            Source = Enumerable.Empty<T>();
        }

        private PagedResult(IEnumerable<T> source, int currentPage,
            int pageSize, int totalPages, int totalCount)
        {
            Source = source;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalCount = totalCount;
        }

        public static PagedResult<T> Create(IEnumerable<T> items,
            int currentPage, int pageSize, int totalPages, int totalCount)
            => new PagedResult<T>(items, currentPage, pageSize, totalPages, totalCount);

        public static PagedResult<T> Empty => new PagedResult<T>();
    }
}