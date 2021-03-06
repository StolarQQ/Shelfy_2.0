﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Shelfy.Infrastructure.Commands.Book;
using Shelfy.Infrastructure.DTO.Book;
using Shelfy.Infrastructure.Pagination;

namespace Shelfy.Infrastructure.Services
{
    public interface IBookService : IService
    {
        Task<BookDetailsDto> GetByIdAsync(Guid id);
        Task<BookDetailsDto> GetByIsbnAsync(string isbn);
        Task<PagedResult<BookDto>> BrowseAsync(int currentPage = 1,
            int pageSize = 5, string query = "");
        Task<IEnumerable<BookDto>> GetAllBooks();
        Task AddAsync(Guid bookId, string title, string originalTitle,string description,
            string isbn, string cover, int pages, string publisher,
            DateTime publishedAt, IEnumerable<Guid> authorsId, Guid userId);
        Task UpdateAsync(Guid id, JsonPatchDocument<UpdateBook> patchBook);
        Task DeleteAsync(Guid id);
    }
}