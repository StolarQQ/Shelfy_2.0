﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Shelfy.Core.Helper;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.DTO.Book;

namespace Shelfy.Infrastructure.Services
{
    public interface IBookService
    {
        Task<BookDetailsDto> GetAsync(Guid id);
        Task<BookDetailsDto> GetAsync(string title);
        Task<PagedResult<BookDto>> BrowseAsync(int currentPage = 1, int pageSize = 5);
        Task AddAsync(string title, string originalTitle,string description,
            string isbn, string cover, int pages, string publisher,
            DateTime publishedAt, IEnumerable<Guid> authorsId, Guid userId);
        Task UpdateAsync(Guid id, JsonPatchDocument<UpdateBook> patchBook);
        Task DeleteAsync(Guid id);
    }
}