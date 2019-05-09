using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.DTO.Author;
using Shelfy.Infrastructure.Services;
using Xunit;

namespace Shelfy.Tests.Services
{
    public class AuthorServiceTests
    {
        [Fact]
        public async Task getByIdAsync_should_invoke_getByIdAsync_on_authorRepository()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var defaultAuthorImage = "https://www.stolarstate.pl/avatar/author/default.png";
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            var author = new Author(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);
            var authorDto = new AuthorDto
            {
                AuthorId = author.AuthorId,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Description = author.Description,
                AuthorSource = author.AuthorSource,
                AuthorWebsite = author.AuthorWebsite,
                BirthPlace = author.BirthPlace,
                DateOfBirth = author.DateOfBirth,
                ImageUrl = author.ImageUrl,
                DateOfDeath = author.DateOfDeath,
                UserId = author.UserId
            };
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<AuthorDto>(author)).Returns(authorDto);
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object);
            authorRepositoryMock.Setup(x => x.GetByIdAsync(author.AuthorId)).ReturnsAsync(author);

            // Act
            var existingAuthorDto = await authorService.GetByIdAsync(author.AuthorId);

            // Assert
            authorRepositoryMock.Verify(x => x.GetByIdAsync(authorId), Times.Once);
            existingAuthorDto.Should().NotBeNull();
            existingAuthorDto.FirstName.Should().BeEquivalentTo(author.FirstName);
        }

        [Fact]
        public async Task getByIdAsync_should_thrown_exception_for_not_existing_authorId()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var defaultAuthorImage = "https://www.stolarstate.pl/avatar/author/default.png";
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            var author = new Author(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object);
            authorRepositoryMock.Setup(x => x.GetByIdAsync(author.AuthorId)).ReturnsAsync(author);
            var notExistingAuthorId = Guid.NewGuid();
            var expectedExMessage = $"Author with id '{notExistingAuthorId}' was not found.";

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await authorService.GetByIdAsync(notExistingAuthorId));

            // Assert
            authorRepositoryMock.Verify(x => x.GetByIdAsync(notExistingAuthorId), Times.Once);
            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().BeEquivalentTo(expectedExMessage);
        }

        [Fact]
        public async Task registerAsync_should_invoke_add_async_once_on_authorRepository()
        {
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var authorId = Guid.NewGuid();
            var defaultAuthorImage = "https://www.stolarstate.pl/avatar/author/default.png";
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            var userId = Guid.NewGuid();
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object);

            await authorService.RegisterAsync(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);

            authorRepositoryMock.Verify(x => x.GetByIdAsync(authorId), Times.Once);
            authorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Author>()), Times.Once);
        }

        [Fact]
        public async Task registerAsync_should_never_invoke_addAsync_with_id_existing_in_database()
        {
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var authorId = Guid.NewGuid();
            var defaultAuthorImage = "https://www.stolarstate.pl/avatar/author/default.png";
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            var userId = Guid.NewGuid();
            var existingAuthor = new Author(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object);

            await authorService.RegisterAsync(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);

            authorRepositoryMock.Setup(x => x.GetByIdAsync(authorId)).ReturnsAsync(existingAuthor);
            authorRepositoryMock.Verify(x => x.AddAsync(existingAuthor), Times.Never);
        }

        [Fact]
        public async Task registerAsync_should_thrown_exception_with_incorrect_avatar_url()
        {
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var authorId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            var incorrectImageUrl = "wwww.stolarstate.pl/avatar/author/default.pl";
            var expectedExMessage = $"URL {incorrectImageUrl} doesn't meet required criteria";
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await authorService.RegisterAsync
            (authorId, "Jon", "Skeet", "C# in Depth Author", incorrectImageUrl, new DateTime(1984, 01, 01),
                null, "Texas", authorWebsite, authorWebsite, userId));

            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().BeEquivalentTo(expectedExMessage);
        }


        [Fact]
        public async Task deleteAsync_should_invoke_removeAsync_on_authorRepository_for_exist_author()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var defaultAuthorImage = "https://www.stolarstate.pl/avatar/author/default.png";
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            var author = new Author(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object);
            authorRepositoryMock.Setup(x => x.GetByIdAsync(author.AuthorId)).ReturnsAsync(author);

            // Act
            await authorService.DeleteAsync(author.AuthorId);

            // Assert
            authorRepositoryMock.Verify(x => x.GetByIdAsync(author.AuthorId), Times.Once);
            authorRepositoryMock.Verify(x => x.RemoveAsync(author.AuthorId), Times.Once);
        }

        // TODO FIX
        [Fact]
        public async Task deleteAsync_should_never_invoke_removeAsync_on_authorRepository_for_exist_author()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var defaultAuthorImage = "https://www.stolarstate.pl/avatar/author/default.png";
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            var author = new Author(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object);
            var notExistAuthorId = Guid.NewGuid();

            // Act
            await authorService.DeleteAsync(notExistAuthorId);


            // Assert
            authorRepositoryMock.Setup(x => x.GetByIdAsync(authorId)).ReturnsAsync(author);
            authorRepositoryMock.Verify(x => x.RemoveAsync(author.AuthorId), Times.Never);
        }
    }
}
