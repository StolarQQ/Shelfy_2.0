﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shelfy.Core.Domain;
using Shelfy.Core.Exceptions;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.DTO.Jwt;
using Shelfy.Infrastructure.DTO.User;
using Shelfy.Infrastructure.Exceptions;
using Shelfy.Infrastructure.Extensions;
using Shelfy.Infrastructure.Pagination;
using Shelfy.Infrastructure.Validators;
using ErrorCodes = Shelfy.Infrastructure.Exceptions.ErrorCodes;

namespace Shelfy.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypterService _encrypterService;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IMemoryCache _cache;
        private readonly ICredentialValidator _credentialValidator;

        public UserService(IUserRepository userRepository, IEncrypterService encrypterService,
            IJwtHandler jwtHandler, IMapper mapper, ILogger<UserService> logger, IMemoryCache cache, ICredentialValidator credentialValidator)
        {
            _userRepository = userRepository;
            _encrypterService = encrypterService;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _credentialValidator = credentialValidator;
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

        public async Task<PagedResult<UserDto>> BrowseAsync(int pageNumber, int pageSize)
        {
            var users = await _userRepository.BrowseAsync(pageNumber, pageSize);

            return _mapper.Map<PagedResult<UserDto>>(users);
        }

        public async Task RegisterAsync(Guid userid, string email, string username,
            string password, Role role = Role.User)
        {
            var user = await _userRepository.GetByEmailAsync(email.ToLowerInvariant());
            if (user != null)
            {
                throw new ServiceException(ErrorCodes.EmailInUse, $"User with email '{email}' already exist.");
            }

            user = await _userRepository.GetByUsernameAsync(username.ToLowerInvariant());
            if (user != null)
            {
                throw new ServiceException(ErrorCodes.UsernameInUse, $"User with username '{username}' already exist.");
            }
            
            var validationResult = _credentialValidator.ValidatePassword(password);
            if (validationResult.IsValid == false)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, validationResult.ValidationMessage.MergeResults());
            }

            var salt = _encrypterService.GetSalt();
            var hash = _encrypterService.GetHash(password, salt);

            // Logic component ?? How to handle object with multiple parameters ??
            try
            {
                user = new User(userid, email, username, hash, salt, role, "https://www.stolarstate.pl/avatar/user/default.png");
            }
            catch (DomainException ex)
            {
                throw new ServiceException(ex, ErrorCodes.InvalidInput, ex.Message);
            }

            _logger.LogWarning($"User with email '{user.Email}' as role '{user.Role}' was created at '{user.CreatedAt}'");

            await _userRepository.AddAsync(user);
        }

        public async Task LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials, try again.");
            }

            var hash = _encrypterService.GetHash(password, user.Salt);
            if (user.Password != hash)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials, try again.");
            }

            var jwt = _jwtHandler.CreateToken(user.UserId, user.Role);

            var jwtDto = new TokenDto
            {
                Token = jwt.Token,
                Role = user.Role,
                Expires = jwt.Expires
            };

            _cache.Set(user.Email, jwtDto, TimeSpan.FromMinutes(5));
            _logger.LogInformation($"User '{user.Username}' logged in.");
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
            
            var hash = _encrypterService.GetHash(oldPassword, user.Salt);

            if (user.Password != hash)
            {
                throw new ServiceException(ErrorCodes.InvalidPassword, "Your current password is incorrect, try again.");
            }

            var validationResult = _credentialValidator.ValidatePassword(newPassword);

            if (validationResult.IsValid == false)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, validationResult.ValidationMessage.MergeResults());
            }

            var newHash = _encrypterService.GetHash(newPassword, user.Salt);
            user.SetPassword(newHash);

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"User '{user.Username}' changed password.");
        }

        public async Task SetAvatar(Guid id, string avatar)
        {
            var user = await _userRepository.GetOrFailAsync(id);

            try
            {
                user.SetAvatar(avatar);
            }
            catch (DomainException ex)
            {
                throw new ServiceException(ex, ErrorCodes.InvalidInput, ex.Message);
            }

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"User '{user.Username}' set up new avatar.");
        }

        public async Task DeleteAvatar(Guid id)
        {
            var user = await _userRepository.GetOrFailAsync(id);

            try
            {
                user.DeleteAvatar();
            }
            catch (DomainException ex)
            {
                throw new ServiceException(ex, ErrorCodes.InvalidInput, ex.Message);
            }

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"User '{user.Username}' deleted avatar.");
        }
    }
}
