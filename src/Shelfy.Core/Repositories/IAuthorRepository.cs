using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelfy.Core.Domain;

namespace Shelfy.Core.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author> GetByIdAsync(Guid id);
        Task<IEnumerable<Author>> BrowseByPhraseAsync(string phrase);
        Task<IEnumerable<Author>> BrowseAsync();
        Task AddAsync(Author author);
        Task UpdateAsync(Author author);
        Task RemoveAsync(Guid id);
    }
}