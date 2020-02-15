using System;
using System.Threading.Tasks;
using Shelfy.Core.Domain;
using Shelfy.Core.Helper;
using Shelfy.Infrastructure.DTO.User;
using Shelfy.Infrastructure.Extensions;
using Shelfy.Infrastructure.Pagination;

namespace Shelfy.Infrastructure.Services
{
    public interface IUserService : IService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto> GetByUserNameAsync(string username);
        Task<PagedResult<UserDto>> BrowseAsync(int pageNumber = 1, int pageSize = 5);
        Task RegisterAsync(Guid userid, string email, string username,
             string password, Role role = Role.User);
        Task LoginAsync(string email, string password);
        Task DeleteAsync(Guid id);
        Task ChangePassword(Guid id, string oldPassword, string newPassword);
        Task SetAvatar(Guid id, string avatar);
        Task DeleteAvatar(Guid id);

    }
}