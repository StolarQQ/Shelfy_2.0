using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelfy.Core.Domain;
using Shelfy.Infrastructure.DTO;
using Shelfy.Infrastructure.DTO.Book;

namespace Shelfy.Infrastructure.Services
{
    public interface IBookService
    {
        Task<BookDto> GetAsync(Guid id);
        Task<BookDto> GetAsync(string title);
        Task<IEnumerable<BookDto>> BrowseAsync();
        Task AddAsync(string title, string originalTitle,string description,
            string isbn, int pages, string publisher, DateTime publishedAt);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Guid id);
    }
}