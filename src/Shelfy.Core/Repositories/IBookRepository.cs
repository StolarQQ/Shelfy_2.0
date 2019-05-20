using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelfy.Core.Domain;
using Shelfy.Core.Helper;

namespace Shelfy.Core.Repositories
{
    public interface IBookRepository
    {
        Task<Book> GetByIdAsync(Guid id);
        Task<Book> GetByIsbnAsync(string isbn);
        Task<IEnumerable<Review>> GetBooksReviews();
        Task<PagedResult<Book>> BrowseAsync(int currentPage, int pageSize);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task RemoveAsync(Guid id);
    }
}