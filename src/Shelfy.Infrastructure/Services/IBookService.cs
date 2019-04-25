using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.DTO.Book;

namespace Shelfy.Infrastructure.Services
{
    public interface IBookService
    {
        Task<BookDto> GetAsync(Guid id);
        Task<BookDto> GetAsync(string title);
        Task<IEnumerable<BookDto>> BrowseAsync();
        Task AddAsync(string title, string originalTitle,string description,
            string isbn, int pages, string publisher, DateTime publishedAt, IEnumerable<Guid> authorsId);
        Task UpdateAsync(Guid id, JsonPatchDocument<UpdateBook> patchBook);
        Task DeleteAsync(Guid id);
    }
}