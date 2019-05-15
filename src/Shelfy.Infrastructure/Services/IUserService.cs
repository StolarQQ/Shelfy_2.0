using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelfy.Core.Types;
using Shelfy.Infrastructure.DTO.Jwt;
using Shelfy.Infrastructure.DTO.User;
using Shelfy.Infrastructure.Helper;

namespace Shelfy.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto> GetByUserNameAsync(string username);
        Task<PagedResult<UserDto>> BrowseAsync(int pageNumber = 1, int pageSize = 5);
        Task RegisterAsync(Guid userid, string email, string username,
             string password);
        Task<TokenDto> LoginAsync(string email, string password);
        Task DeleteAsync(Guid id);
        Task ChangePassword(Guid id, string oldPassword, string newPassword);
        Task SetAvatar(Guid id, string avatar);
    }
}