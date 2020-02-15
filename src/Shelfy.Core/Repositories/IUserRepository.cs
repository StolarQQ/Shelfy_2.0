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
        Task<IPagedResult<User>> BrowseAsync(int currentPage = 1, int pageSize = 5);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task RemoveAsync(Guid id);
    }
}