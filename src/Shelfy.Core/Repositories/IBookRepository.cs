using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelfy.Core.Domain;

namespace Shelfy.Core.Repositories
{
    public interface IBookRepository
    {
        Task<Book> GetByIdAsync(Guid id);
        Task<Book> GetByIsbnAsync(string isbn);
        Task<IEnumerable<Book>> BrowseAsync();
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task RemoveAsync(Guid id);
    }
}