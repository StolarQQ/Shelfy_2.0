using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelfy.Core.Domain;

namespace Shelfy.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> BrowseAsync(string name = "");
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task RemoveAsync(Guid id);
    }
}