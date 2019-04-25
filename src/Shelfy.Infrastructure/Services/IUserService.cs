using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelfy.Infrastructure.DTO.User;

namespace Shelfy.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto> GetByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllAsync(string name);
        Task RegisterAsync(Guid userid, string email, string username,
             string password, string salt, string imgUrl, string role = "user");
    }
}