using System.Collections.Generic;
using System.Linq;

namespace Shelfy.Infrastructure.Helper
{
    //public class PagedResult<T>
    //{
    //    //public IEnumerable<T> Source { get; }
    //    //public int CurrentPage { get; private set; } = 1;
    //    //public int PageSize { get; private set; } = 10;
    //    //public int TotalPages { get; private set; }
    //    //public int TotalCount { get; private set; }

    //    //public bool HasPrevious => CurrentPage > 1;
    //    //public bool HasNext => CurrentPage < TotalPages;
    //    //public bool ShowFirst => CurrentPage != 1;
    //    //public bool ShowLast => CurrentPage != TotalPages;

    //    //private PagedResult()
    //    //{
    //    //    Source = Enumerable.Empty<T>();
    //    //}

    //    //private PagedResult(IEnumerable<T> items, int totalPages,
    //    //    int currentPage, int pageSize, int totalCount)
    //    //{
    //    //    TotalPages = totalPages;
    //    //    PageSize = pageSize;
    //    //    CurrentPage = currentPage;
    //    //    TotalCount = totalCount;
    //    //    Source = items;
    //    //}

    //    //public static PagedResult<T> Create(IEnumerable<T> items, int totalPages,
    //    //    int currentPage, int pageSize, int totalCount)
    //    //    => new PagedResult<T>(items, totalPages, currentPage, pageSize, totalCount);

    //    //public static PagedResult<T> Empty => new PagedResult<T>();
    //}
}