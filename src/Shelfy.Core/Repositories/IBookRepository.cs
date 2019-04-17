using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelfy.Core.Domain;

namespace Shelfy.Core.Repositories
{
    public interface IBookRepository
    {
        Task<Book> GetAsync(Guid id);
        Task<Book> GetAsync(string title);
        Task<IEnumerable<Book>> BrowseAsync();
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task RemoveAsync(Guid id);
    }
}