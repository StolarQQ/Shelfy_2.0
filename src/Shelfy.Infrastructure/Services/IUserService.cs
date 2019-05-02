using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.DTO;
using Shelfy.Infrastructure.DTO.Jwt;
using Shelfy.Infrastructure.DTO.User;

namespace Shelfy.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto> GetByUserNameAsync(string username);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task RegisterAsync(Guid userid, string email, string username,
             string password);
        Task<TokenDto> LoginAsync(string email, string password);
        Task DeleteAsync(Guid id);
        Task ChangePassword(Guid id, string oldPassword, string newPassword);
        Task SetAvatar(Guid id, string avatar);
    }
}