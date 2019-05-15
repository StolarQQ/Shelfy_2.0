using System;
using FluentAssertions;
using Shelfy.Core.Domain;
using Xunit;

namespace Shelfy.Tests.Domain
{
    public class AuthorDomainTests
    {
        [Fact]
        public void setFirstName_should_assign_correct_firstName_to_author()
        {
            // Arrange
            var author = CorrectAuthor();
            var expectedFirstName = "Micheal";

            // Act
            author.SetFirstName(expectedFirstName);

            // Assert
            author.FirstName.Should().BeEquivalentTo(expectedFirstName);
        }

        [Fact]
        public void setFirstName_should_thrown_exception_when_firstName_is_null()
        {
            // Arrange
            var author = CorrectAuthor();
            string expectedFirstName = null;
            var expectedExMsg = $"Author with {author.AuthorId} cannot have an empty FirstName";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => author.SetFirstName(expectedFirstName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setFirstName_should_thrown_exception_when_firstName_is_whitespace()
        {
            // Arrange
            var author = CorrectAuthor();
            var expectedFirstName = "";
            var expectedExMsg = $"Author with {author.AuthorId} cannot have an empty FirstName";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => author.SetFirstName(expectedFirstName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        private Author CorrectAuthor()
        {
            var authorId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var defaultAuthorImage = "https://www.stolarstate.pl/avatar/author/default.png";
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            var author = new Author(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);

            return author;
        }
    }
}