using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using Newtonsoft.Json;
using Shelfy.Core.Domain;
using Shelfy.Core.Exceptions;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.Commands.Author;
using Shelfy.Infrastructure.DTO.Author;
using Shelfy.Infrastructure.Exceptions;
using Shelfy.Infrastructure.Services;
using Xunit;

namespace Shelfy.Tests.Services
{
    public class AuthorServiceTests
    {
        private readonly Author _author;

        public AuthorServiceTests()
        {
            var authorId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var defaultAuthorImage = "https://www.stolarstate.pl/avatar/author/default.png";
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            _author = new Author(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);
        }

        [Fact]
        public async Task getById_async_should_invoke_getById_on_author_repository()
        {
            // Arrange
            var authorDto = new AuthorDto
            {
                AuthorId = _author.AuthorId,
                FirstName = _author.FirstName,
                LastName = _author.LastName,
                Description = _author.Description,
                AuthorSource = _author.AuthorSource,
                AuthorWebsite = _author.AuthorWebsite,
                BirthPlace = _author.BirthPlace,
                DateOfBirth = _author.DateOfBirth,
                ImageUrl = _author.ImageUrl,
                DateOfDeath = _author.DateOfDeath,
                UserId = _author.CreatorId
            };
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var validatorMock = new Mock<IValidator<Author>>();
            mapperMock.Setup(x => x.Map<AuthorDto>(_author)).Returns(authorDto);
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object, validatorMock.Object);
            authorRepositoryMock.Setup(x => x.GetByIdAsync(_author.AuthorId)).ReturnsAsync(_author);

            // Act
            var existingAuthorDto = await authorService.GetByIdAsync(_author.AuthorId);

            // Assert
            authorRepositoryMock.Verify(x => x.GetByIdAsync(_author.AuthorId), Times.Once);
            existingAuthorDto.Should().NotBeNull();
            existingAuthorDto.FirstName.Should().BeEquivalentTo(_author.FirstName);
        }

        [Fact]
        public async Task getByIdAsync_should_thrown_exception_for_not_existing_authorId()
        {
            // Arrange
            var notExistingAuthorId = Guid.NewGuid();
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var validatorMock = new Mock<IValidator<Author>>();
            authorRepositoryMock.Setup(x => x.GetByIdAsync(_author.AuthorId)).ReturnsAsync(_author);
            var expectedExMessage = $"Author with id '{notExistingAuthorId}' was not found.";
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object, validatorMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(async () => await authorService.GetByIdAsync(notExistingAuthorId));
            authorRepositoryMock.Verify(x => x.GetByIdAsync(notExistingAuthorId), Times.Once);
            exception.Message.Should().BeEquivalentTo(expectedExMessage);
        }

        [Fact]
        public async Task registerAsync_should_invoke_add_async_once_on_author_repository()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var defaultAuthorImage = "https://www.stolarstate.pl/avatar/author/default.png";
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            var userId = Guid.NewGuid();
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var validatorMock = new Mock<IValidator<Author>>();
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object, validatorMock.Object);

            // Act
            await authorService.RegisterAsync(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);

            // Assert
            authorRepositoryMock.Verify(x => x.GetByIdAsync(authorId), Times.Once);
            authorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Author>()), Times.Once);
        }

        [Fact]
        public async Task registerAsync_should_never_invoke_addAsync_with_id_existing_in_database()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var defaultAuthorImage = "https://www.stolarstate.pl/avatar/author/default.png";
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            var existingAuthor = new Author(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var validatorMock = new Mock<IValidator<Author>>();
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object, validatorMock.Object);

            // Act
            await authorService.RegisterAsync(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);

            // Arrange
            authorRepositoryMock.Setup(x => x.GetByIdAsync(authorId)).ReturnsAsync(existingAuthor);
            authorRepositoryMock.Verify(x => x.AddAsync(existingAuthor), Times.Never);
        }

        [Fact]
        public async Task registerAsync_should_thrown_exception_with_incorrect_avatar_url()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            var incorrectImageUrl = "wwww.stolarstate.pl/avatar/author/default.pl";
            var expectedExMessage = $"URL {incorrectImageUrl} doesn't meet required criteria.";
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var validatorMock = new Mock<IValidator<Author>>();
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object, validatorMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(async () => await authorService.RegisterAsync
            (authorId, "Jon", "Skeet", "C# in Depth Author", incorrectImageUrl, new DateTime(1984, 01, 01),
                null, "Texas", authorWebsite, authorWebsite, userId));
            exception.Message.Should().BeEquivalentTo(expectedExMessage);
            exception.InnerException.Should().BeOfType<DomainException>();
        }

        [Fact]
        public async Task updateAsync_should_should_invoke_updateAsync_on_author_repository_with_correct_data()
        {
            // Arrange
            var updateAuthor = new UpdateAuthor
            {
                FirstName = _author.FirstName,
                LastName = _author.LastName,
                Description = _author.Description,
                ImageUrl = _author.ImageUrl,
                DateOfBirth = _author.DateOfBirth,
                DateOfDeath = _author.DateOfDeath,
                BirthPlace = _author.BirthPlace,
                AuthorWebsite = _author.AuthorWebsite,
                AuthorSource = _author.AuthorSource
            };
            var mapperMock = new Mock<IMapper>();
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var validatorMock = new Mock<IValidator<Author>>();
            authorRepositoryMock.Setup(x => x.GetByIdAsync(_author.AuthorId)).ReturnsAsync(_author);
            mapperMock.Setup(x => x.Map<UpdateAuthor>(_author)).Returns(updateAuthor);
            mapperMock.Setup(x => x.Map(It.IsAny<UpdateAuthor>(), It.IsAny<Author>())).Returns(_author);

            var jsonDoc = "[{\"op\":\"replace\",\"path\":\"/FirstName\",\"value\":\"Marcelo\"}]";
            var partialUpdate = JsonConvert.DeserializeObject<JsonPatchDocument<UpdateAuthor>>(jsonDoc);
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object, validatorMock.Object);

            // Act
            await authorService.UpdateAsync(_author.AuthorId, partialUpdate);

            // Assert
            authorRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Author>()), Times.Once());
        }

        [Fact]
        public async Task updateAsync_should_should_never_invoke_updateAsync_on_author_repository_with_not_exist_author_id()
        {
            // Arrange
            var notExistAuthorId = Guid.NewGuid();
            var expectedExMessage = $"Author with id '{notExistAuthorId}' was not found.";
            var mapperMock = new Mock<IMapper>();
            var validatorMock = new Mock<IValidator<Author>>();
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            authorRepositoryMock.Setup(x => x.GetByIdAsync(_author.AuthorId)).ReturnsAsync(_author);

            var serialized = "[{\"op\":\"replace\",\"path\":\"/FirstName\",\"value\":\"Test\"}]";
            var partialUpdate = JsonConvert.DeserializeObject<JsonPatchDocument<UpdateAuthor>>(serialized);
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object, validatorMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(async () =>
                await authorService.UpdateAsync(notExistAuthorId, partialUpdate));
            authorRepositoryMock.Verify(x => x.RemoveAsync(_author.AuthorId), Times.Never);
            exception.Message.Should().BeEquivalentTo(expectedExMessage);
        }

        [Fact]
        public async Task deleteAsync_should_invoke_removeAsync_on_authorRepository_for_exist_author()
        {
            // Arrange
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var validatorMock = new Mock<IValidator<Author>>();
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object, validatorMock.Object);
            authorRepositoryMock.Setup(x => x.GetByIdAsync(_author.AuthorId)).ReturnsAsync(_author);

            // Act
            await authorService.DeleteAsync(_author.AuthorId);

            // Assert
            authorRepositoryMock.Verify(x => x.GetByIdAsync(_author.AuthorId), Times.Once);
            authorRepositoryMock.Verify(x => x.RemoveAsync(_author.AuthorId), Times.Once);
        }

        // TODO FIX
        [Fact]
        public async Task deleteAsync_should_never_invoke_removeAsync_on_authorRepository_for_not_exist_author()
        {
            // Arrange
            var notExistAuthorId = Guid.NewGuid();
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var validatorMock = new Mock<IValidator<Author>>();
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object, validatorMock.Object);
            var expectedExMessage = $"Author with id '{notExistAuthorId}' was not found.";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(async () =>
                await authorService.DeleteAsync(notExistAuthorId));
            authorRepositoryMock.Verify(x => x.RemoveAsync(_author.AuthorId), Times.Never);
            exception.Message.Should().BeEquivalentTo(expectedExMessage);
        }
    }
}
