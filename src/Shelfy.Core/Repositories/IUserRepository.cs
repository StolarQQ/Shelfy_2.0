using System;
using System.Threading.Tasks;
using Shelfy.Core.Domain;
using Shelfy.Core.Helper;

namespace Shelfy.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
        Task<PagedResult<User>> BrowseAsync(int currentPage, int pageSize);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task RemoveAsync(Guid id);
    }
}