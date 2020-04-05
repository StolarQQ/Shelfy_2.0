using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.Exceptions;
using Shelfy.Infrastructure.Extensions;
using Xunit;

namespace Shelfy.Tests.Extensions
{
    public class InfrastructureExtensionsTests
    {
        private readonly Book _book;
        private readonly Author _author;
        private readonly User _user;

        public InfrastructureExtensionsTests()
        {
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();
            var bookCover = "https://www.stolarstate.pl/cover/book/default.png";
            _book = new Book(bookId, "C# via Clr", null, "CLR Book via C# by XYZ", "1234567890987",
               bookCover, 346, "Manning", DateTime.Now, userId);


            var authorId = Guid.NewGuid();
            var defaultAuthorImage = "https://www.stolarstate.pl/avatar/author/default.png";
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            _author = new Author(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);

            var userAvatar = "https://www.stolarstate.pl/avatar/user/default.png";

            _user = new User(userId, "test@gmail.com", "UserTest", "Secret",
                "randomsaltxdsa123123dsa", Role.User, userAvatar);
        }

        [Theory]
        [InlineData(null)] 
        [InlineData("")] 
        public void IsEmpty_should_return_true_for_null_or_whitespace(string input)
        {
            //Arrange
            var expectedResult = true;

            //Act
            var result = input.IsEmpty();

            //Assert
            result.Should().Be(expectedResult);
        }
        
        [Fact]
        public void IsEmpty_should_return_false_for_not_empty_input()
        {
            //Arrange
            string emptyString = "ValidString";
            var expectedResult = false;

            //Act
            var result = emptyString.IsEmpty();

            //Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task GetOrFailAsync_for_BookRepository_should_return_book_when_id_exist()
        {
            //Arrange
            var repoMock = new Mock<IBookRepository>();
            repoMock.Setup(x => x.GetByIdAsync(_book.BookId)).ReturnsAsync(_book);

            //Act
            var bookResult = await repoMock.Object.GetOrFailAsync(_book.BookId);

            //Assert
            bookResult.ISBN.Should().BeEquivalentTo(_book.ISBN);
            bookResult.OriginalTitle.Should().BeEquivalentTo(_book.OriginalTitle);
        }

        [Fact]
        public async Task GetOrFailAsync_for_BookRepository_should_thrown_exception_when_book_was_not_found()
        {
            //Arrange
            var notExistingId = Guid.NewGuid();

            var exMsg = $"Book with id '{notExistingId}' was not found.";
            var repoMock = new Mock<IBookRepository>();
            repoMock.Setup(x => x.GetByIdAsync(_book.BookId)).ReturnsAsync(_book);

            //Act & Assert
            var ex = await Assert.ThrowsAsync<ServiceException>(async () => await repoMock.Object.GetOrFailAsync(notExistingId));
            ex.Message.Should().BeEquivalentTo(exMsg);
        }

        [Fact]
        public async Task GetOrFailAsync_for_BookRepository_should_return_book_when_isbn_book_exist()
        {
            //Arrange
            var repoMock = new Mock<IBookRepository>();
            repoMock.Setup(x => x.GetByIsbnAsync(_book.ISBN)).ReturnsAsync(_book);

            //Act
            var bookResult = await repoMock.Object.GetOrFailAsync(_book.ISBN);

            //Assert
            bookResult.Title.Should().BeEquivalentTo(_book.Title);
            bookResult.Cover.Should().BeEquivalentTo(_book.Cover);
        }

        [Fact]
        public async Task GetOrFailAsync_for_BookRepository_should_thrown_exception_when_isbn_book_was_not_found()
        {
            //Arrange
            var notExistingIsbn = "111111111111";
            var exMsg = $"Book with isbn '{notExistingIsbn}' was not found.";
            var repoMock = new Mock<IBookRepository>();
            repoMock.Setup(x => x.GetByIsbnAsync(_book.ISBN)).ReturnsAsync(_book);

            //Act & Assert
            var ex = await Assert.ThrowsAsync<ServiceException>(async () => await repoMock.Object.GetOrFailAsync(notExistingIsbn));
            ex.Message.Should().BeEquivalentTo(exMsg);
        }

        [Fact]
        public async Task GetOrFailAsync_for_AuthorRepository_should_return_author_for_exist_id()
        {
            //Arrange
            var repoMock = new Mock<IAuthorRepository>();
            repoMock.Setup(x => x.GetByIdAsync(_author.AuthorId)).ReturnsAsync(_author);

            //Act
            var authorResult = await repoMock.Object.GetOrFailAsync(_author.AuthorId);

            //Assert
            authorResult.AuthorSource.Should().BeEquivalentTo(_author.AuthorSource);
            authorResult.Should().NotBeNull();
        }

        [Fact]
        public async Task GetOrFailAsync_for_AuthorRepository_should_thrown_exception_for_not_exist_author()
        {
            //Arrange
            var notExistingId = Guid.NewGuid();
            var exMsg = $"Author with id '{notExistingId}' was not found.";
            var repoMock = new Mock<IAuthorRepository>();
            repoMock.Setup(x => x.GetByIdAsync(_author.AuthorId)).ReturnsAsync(_author);

            //Act & Assert
            var ex = await Assert.ThrowsAsync<ServiceException>(async () => await repoMock.Object.GetOrFailAsync(notExistingId));
            ex.Message.Should().BeEquivalentTo(exMsg);
        }
        [Fact]
        public async Task GetOrFailAsync_for_UserRepository_should_return_user_for_exist_id()
        {
            //Arrange
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(x => x.GetByIdAsync(_user.UserId)).ReturnsAsync(_user);

            //Act
            var userResult = await repoMock.Object.GetOrFailAsync(_user.UserId);

            //Assert
            userResult.Email.Should().BeEquivalentTo(_user.Email);
            userResult.Should().NotBeNull();
        }
        
        [Fact]
        public async Task GetOrFailAsync_for_UserRepository_should_thrown_exception_for_not_exist_id()
        {
            //Arrange
            var notExistingId = Guid.NewGuid();
            var exMsg = $"User with id '{notExistingId}' was not found.";
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(x => x.GetByIdAsync(_user.UserId)).ReturnsAsync(_user);

            //Act & Assert
            var ex = await Assert.ThrowsAsync<ServiceException>(async () => await repoMock.Object.GetOrFailAsync(notExistingId));
            ex.Message.Should().BeEquivalentTo(exMsg);
        }

        [Fact]
        public async Task GetOrFailAsync_for_UserRepository_should_return_user_for_exist_username()
        {
            //Arrange
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(x => x.GetByUsernameAsync(_user.Username)).ReturnsAsync(_user);

            //Act
            var userResult = await repoMock.Object.GetOrFailAsync(_user.Username);

            //Assert
            userResult.Email.Should().BeEquivalentTo(_user.Email);
            userResult.Should().NotBeNull();
        }

        [Fact]
        public async Task GetOrFailAsync_for_UserRepository_should_thrown_exception_for_not_exist_username()
        {
            //Arrange
            var notExistingUsername = "Notexisting";
            var exMsg = $"User with username '{notExistingUsername}' was not found.";
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(x => x.GetByUsernameAsync(_user.Username)).ReturnsAsync(_user);

            //Act & Assert
            var ex = await Assert.ThrowsAsync<ServiceException>(async () => await repoMock.Object.GetOrFailAsync(notExistingUsername));
            ex.Message.Should().BeEquivalentTo(exMsg);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DefaultBookCoverNotEmpty_should_assign_default_cover_when_input_is_null_or_whitespace(string cover)
        {
            //Arrange
            var defaultCover = "https://www.stolarstate.pl/avatar/book/default.png";

            //Act
            var result = cover.SetUpDefaultCoverWhenEmpty();

            //Assert
            result.Should().BeEquivalentTo(defaultCover);
        }
        
        [Fact]
        public void DefaultBookCoverNotEmpty_return_input_when_input_is_valid()
        {
            //Arrange
            var validCover = "https://www.stolarstate.pl/avatar/book/valid.jpeg";

            //Act
            var result = validCover.SetUpDefaultCoverWhenEmpty();

            //Assert
            result.Should().BeEquivalentTo(validCover);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DefaultAuthorImageNotEmpty_should_assign_default_author_image_when_input_is_null_or_whitespace(string authorImg)
        {
            //Arrange
            var defaultCover = "https://www.stolarstate.pl/avatar/author/default.png";

            //Act
            var result = authorImg.DefaultAuthorImageNotEmpty();

            //Assert
            result.Should().BeEquivalentTo(defaultCover);
        }

        [Fact]
        public void DefaultAuthorImageNotEmpty_return_input_when_input_is_valid()
        {
            //Arrange
            var validCover = "https://www.stolarstate.pl/avatar/author/default.jpeg";

            //Act
            var result = validCover.DefaultAuthorImageNotEmpty();

            //Assert
            result.Should().BeEquivalentTo(validCover);
        }
    }
}
