using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.Services;
using Xunit;

namespace Shelfy.Tests
{
    public class AuthorServiceTests
    {
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
        public async Task registerAsync_should_never_invoke_addAsync_with_incorrect_avatar_url()
        {
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            var mapperMock = new Mock<IMapper>();
            var authorId = Guid.NewGuid();
            var defaultAuthorImage = "wwww.stolarstate.pl/avatar/author/default.pl";
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            var userId = Guid.NewGuid();
            var exMsg = $"URL {defaultAuthorImage} doesn't meet required criteria";


            var authorService = new AuthorService(authorRepositoryMock.Object, mapperMock.Object);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await authorService.RegisterAsync
            (authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage, new DateTime(1984, 01, 01),
                null, "Texas", authorWebsite, authorWebsite, userId));

            ex.Should().BeOfType<ArgumentException>();
            exMsg.Should().BeEquivalentTo(ex.Message);
        }
    }
}
