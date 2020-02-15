using System.Collections.Generic;

namespace Shelfy.Core.Pagination
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
}
   