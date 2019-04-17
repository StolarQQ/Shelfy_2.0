using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelfy.Core.Domain;

namespace Shelfy.Core.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author> GetAsync(Guid id);
        Task<Author> GetAsync(string lastName);
        Task<IEnumerable<Author>> BrowseAsync();
        Task AddAsync(Author author);
        Task UpdateAsync(Author author);
        Task RemoveAsync(Guid id);
    }
}