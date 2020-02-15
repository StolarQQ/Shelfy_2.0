using System.Collections.Generic;

namespace Shelfy.Core.Helper
{
    public interface IPagedResult<T>
    {
        IEnumerable<T> Source { get; set; }
        int CurrentPage { get; }
        int PageSize { get; }
        int TotalPages { get; }
        int TotalCount { get; }
        bool HasPrevious { get; }
        bool HasNext { get; }
        bool ShowFirst { get; }
        bool ShowLast { get; }
    }

    /// <summary>
    /// Helper class for pagination
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //public class PagedResult<T> : IPagedResult<T>
    //{
    //    public IEnumerable<T> Source { get; set; }
    //    public int CurrentPage { get; } = 1;
    //    public int PageSize { get; } = 5;
    //    public int TotalPages { get; }
    //    public int TotalCount { get; }

    //    public bool HasPrevious => CurrentPage > 1;
    //    public bool HasNext => CurrentPage < TotalPages;
    //    public bool ShowFirst => CurrentPage != 1;
    //    public bool ShowLast => CurrentPage != TotalPages;

    //    private PagedResult()
    //    {
    //        Source = Enumerable.Empty<T>();
    //    }

    //    private PagedResult(IEnumerable<T> source, int currentPage,
    //        int pageSize, int totalPages, int totalCount)
    //    {
    //        Source = source;
    //        CurrentPage = currentPage;
    //        PageSize = pageSize;
    //        TotalPages = totalPages;
    //        TotalCount = totalCount;
    //    }

    //    public static PagedResult<T> Create(IEnumerable<T> items,
    //        int currentPage, int pageSize, int totalPages, int totalCount)
    //        => new PagedResult<T>(items, currentPage, pageSize, totalPages, totalCount);

    //    public static PagedResult<T> Empty => new PagedResult<T>();
    //}
}
   