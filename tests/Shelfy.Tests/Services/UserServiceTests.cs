using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.DTO.User;
using Shelfy.Infrastructure.Exceptions;
using Shelfy.Infrastructure.Pagination;
using Shelfy.Infrastructure.Services;
using Xunit;

namespace Shelfy.Tests.Services
{
    public class UserServiceTests
    {
        private readonly User _user;

        public UserServiceTests()
        {
            var userId = Guid.NewGuid();
            _user = new User(userId, "test@test.com", "username", "password123",
                "salt123", Role.User, "https://www.test.com/avatar.png");
        }

        [Fact]
        public async Task getById_should_invoke_getById_on_user_repositories()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            var encrypterMock = new Mock<IEncrypterService>();
            var jwtMock = new Mock<IJwtHandler>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<UserService>>();
            var cacheMock = new Mock<IMemoryCache>();
            var userService = new UserService(repoMock.Object, encrypterMock.Object,
                jwtMock.Object, mapperMock.Object, loggerMock.Object, cacheMock.Object);
            repoMock.Setup(x => x.GetByIdAsync(_user.UserId)).ReturnsAsync(_user);

            // Act
            await userService.GetByIdAsync(_user.UserId);

            // Assert
            repoMock.Verify(x => x.GetByIdAsync(_user.UserId), Times.Once);
        }

        [Fact]
        public async Task getById_should_never_invoke_getById_on_userRepository_when_id_not_exist()
        {
            // Arrange
            var newId = Guid.NewGuid();
            var exMsg = $"User with id '{newId}' was not found.";
            var repoMock = new Mock<IUserRepository>();
            var encrypterMock = new Mock<IEncrypterService>();
            var jwtMock = new Mock<IJwtHandler>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<UserService>>();
            var cacheMock = new Mock<IMemoryCache>();
            var userService = new UserService(repoMock.Object, encrypterMock.Object,
                jwtMock.Object, mapperMock.Object, loggerMock.Object, cacheMock.Object);
            repoMock.Setup(x => x.GetByIdAsync(_user.UserId)).ReturnsAsync(_user);

            // Act & Assert
            var exception =
                await Assert.ThrowsAsync<ServiceException>(async () => await userService.GetByIdAsync(newId));
            exception.Message.Should().BeEquivalentTo(exMsg);
            repoMock.Verify(x => x.GetByIdAsync(newId), Times.Once);
        }


        [Fact]
        public async Task getByUserName_should_invoke_getById_on_user_repositories_for_user_that_exist()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            var encrypterMock = new Mock<IEncrypterService>();
            var jwtMock = new Mock<IJwtHandler>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<UserService>>();
            var cacheMock = new Mock<IMemoryCache>();
            var userService = new UserService(repoMock.Object, encrypterMock.Object,
                jwtMock.Object, mapperMock.Object, loggerMock.Object, cacheMock.Object);
            repoMock.Setup(x => x.GetByUsernameAsync(_user.Username)).ReturnsAsync(_user);

            // Act
            await userService.GetByUserNameAsync(_user.Username);

            // Assert
            repoMock.Verify(x => x.GetByUsernameAsync(_user.Username), Times.Once);
        }

        [Fact]
        public async Task
            getByUserName_should_invoke_getById_on_user_repositories_should_never_invoke_getById_on_userRepository_when_id_not_exist()
        {
            // Arrange
            var newUsername = "newUsername";
            var exMsg = $"User with username '{newUsername}' was not found.";
            var repoMock = new Mock<IUserRepository>();
            var encrypterMock = new Mock<IEncrypterService>();
            var jwtMock = new Mock<IJwtHandler>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<UserService>>();
            var cacheMock = new Mock<IMemoryCache>();
            var userService = new UserService(repoMock.Object, encrypterMock.Object,
                jwtMock.Object, mapperMock.Object, loggerMock.Object, cacheMock.Object);
            repoMock.Setup(x => x.GetByUsernameAsync(_user.Username)).ReturnsAsync(_user);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(
                    async () => await userService.GetByUserNameAsync(newUsername));
            exception.Message.Should().BeEquivalentTo(exMsg);
            repoMock.Verify(x => x.GetByUsernameAsync(newUsername), Times.Once);
        }


        [Fact]
        public async Task browseAsync_should_invoke_browseAsync_on_user_repository()
        {
            // Arrange
            var pageSize = 5;
            var currentPage = 1;
            var repoMock = new Mock<IUserRepository>();
            var encrypterMock = new Mock<IEncrypterService>();
            var jwtMock = new Mock<IJwtHandler>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<UserService>>();
            var cacheMock = new Mock<IMemoryCache>();
            var userService = new UserService(repoMock.Object, encrypterMock.Object,
                jwtMock.Object, mapperMock.Object, loggerMock.Object, cacheMock.Object);
            var userPagedResult = PagedResult<User>.Create(null, currentPage, pageSize, 1, 5);
            repoMock.Setup(x => x.BrowseAsync(1, 5)).ReturnsAsync(userPagedResult);
            mapperMock.Setup(x => x.Map<PagedResult<UserDto>>(userPagedResult))
                .Returns(PagedResult<UserDto>.Create(null, currentPage, 5, 1, 5));

            // Act 
            var pagedResult = await userService.BrowseAsync(currentPage, pageSize);

            // Assert
            repoMock.Verify(x => x.BrowseAsync(currentPage, pageSize), Times.Once);
            pagedResult.CurrentPage.Should().Be(currentPage);
            pagedResult.PageSize.Should().Be(pageSize);
        }

        [Fact]
        public async Task deleteAsync_should_invoke_removeAsync_on_user_repository()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            var encrypterMock = new Mock<IEncrypterService>();
            var jwtMock = new Mock<IJwtHandler>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<UserService>>();
            var cacheMock = new Mock<IMemoryCache>();
            var userService = new UserService(repoMock.Object, encrypterMock.Object,
                jwtMock.Object, mapperMock.Object, loggerMock.Object, cacheMock.Object);
            repoMock.Setup(x => x.GetByIdAsync(_user.UserId)).ReturnsAsync(_user);

            // Act 
            await userService.DeleteAsync(_user.UserId);

            // Assert
            
            repoMock.Verify(x => x.RemoveAsync(_user.UserId), Times.Once);
        }

        [Fact]
        public async Task deleteAsync_should_never_invoke_removeAsync_on_user_repository_when_user_not_exist()
        {
            // Arrange
            var newUserId = Guid.NewGuid();
            var repoMock = new Mock<IUserRepository>();
            var encrypterMock = new Mock<IEncrypterService>();
            var jwtMock = new Mock<IJwtHandler>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<UserService>>();
            var cacheMock = new Mock<IMemoryCache>();
            var userService = new UserService(repoMock.Object, encrypterMock.Object,
                jwtMock.Object, mapperMock.Object, loggerMock.Object, cacheMock.Object);
            repoMock.Setup(x => x.GetByIdAsync(_user.UserId)).ReturnsAsync(_user);

            // Act & Assert
            await Assert.ThrowsAsync<ServiceException>(async ()
                => await userService.DeleteAsync(newUserId));
            repoMock.Verify(x => x.RemoveAsync(newUserId), Times.Never);
        }
    }
}