using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.DTO.Jwt;
using Shelfy.Infrastructure.DTO.User;
using Shelfy.Infrastructure.Extensions;
using Shelfy.Infrastructure.Helper;

namespace Shelfy.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypterService _encrypterService;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IEncrypterService encrypterService,
            IJwtHandler jwtHandler, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _encrypterService = encrypterService;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetOrFailAsync(id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetByUserNameAsync(string username)
        {
            var user = await _userRepository.GetOrFailAsync(username);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<PagedResult<UserDto>> BrowseAsync(int pageNumber = 1, int pageSize = 5)
        {
            var users = await _userRepository.BrowseAsync(pageNumber, pageSize);

            var paginatedResult = users.Paginate(pageNumber, pageSize);

            return _mapper.Map<PagedResult<UserDto>>(paginatedResult);
        }

        public async Task RegisterAsync(Guid userid, string email, string username,
            string password)
        {
            var user = await _userRepository.GetByEmailAsync(email.ToLowerInvariant());
            if (user != null)
            {
                throw new Exception($"User with email '{email}' already exist.");
            }

            user = await _userRepository.GetByUsernameAsync(username.ToLowerInvariant());
            if (user != null)
            {
                throw new Exception($"User with username '{username}' already exist.");
            }

            password.PasswordValidation();

            var salt = _encrypterService.GetSalt();
            var hash = _encrypterService.GetHash(password, salt);
            var defaultAvatar = "https://www.stolarstate.pl/avatar/user/default.png";

            user = new User(userid, email, username, hash, salt, Role.User, defaultAvatar);

            _logger.LogWarning($"User with email '{user.Email}' as role '{user.Role}' was created at '{user.CreatedAt}'");

            await _userRepository.AddAsync(user);
        }

        public async Task<TokenDto> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Invalid credentials, try again.");
            }

            var hash = _encrypterService.GetHash(password, user.Salt);
            if (user.Password != hash)
            {
                throw new Exception("Invalid credentials, try again.");
            }

            var jwt = _jwtHandler.CreateToken(user.UserId, user.Role);

            _logger.LogInformation($"User '{user.Username}' logged in.");

            return new TokenDto
            {
                Token = jwt.Token,
                Role = user.Role,
                Expires = jwt.Expires
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetOrFailAsync(id);
            
            await _userRepository.RemoveAsync(user.UserId);

            _logger.LogInformation($"User with id '{user.UserId}' was deleted at '{DateTime.UtcNow}'");
        }

        public async Task ChangePassword(Guid id, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetOrFailAsync(id);

            var salt = user.Salt;
            var hash = _encrypterService.GetHash(oldPassword, salt);

            if (user.Password != hash)
            {
                throw new Exception("Your old password is incorrect, try again.");
            }

            newPassword.PasswordValidation();
            user.SetPassword(newPassword);

            _logger.LogInformation($"User '{user.Username}' changed password");
        }

        public async Task SetAvatar(Guid id, string avatar)
        {
            var user = await _userRepository.GetOrFailAsync(id);
            
            user.SetAvatar(avatar);

            _logger.LogInformation($"User '{user.Username}' set up new avatar");
        }
    }
}
