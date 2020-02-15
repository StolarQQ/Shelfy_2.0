using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelfy.Core.Domain;
using Shelfy.Core.Helper;

namespace Shelfy.Core.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author> GetByIdAsync(Guid id);
        Task<IEnumerable<Author>> BrowseByPhraseAsync(string phrase);
        Task<IPagedResult<Author>> BrowseAsync(int currentPage = 1, int pageSize = 5);
        Task AddAsync(Author author);
        Task UpdateAsync(Author author);
        Task RemoveAsync(Guid id);
    }
}