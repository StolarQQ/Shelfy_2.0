using System;
using AutoFixture;
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
        public void setFirstName_should_assign_correct_firstName_to_author_when_length_is_2()
        {
            // Arrange
            var author = CorrectAuthor();
            string expectedFirstName = "TX";


            // Act
            author.SetFirstName(expectedFirstName);


            //Assert
            author.FirstName.Should().BeEquivalentTo(expectedFirstName);
            expectedFirstName.Length.Should().Be(2);
        }

        [Fact]
        public void setFirstName_should_assign_correct_firstName_to_author_when_length_is_20()
        {
            // Arrange
            var author = CorrectAuthor();
            string expectedFirstName = "ABCDEFGHIJKLMNOPRSTU";

            // Act
            author.SetFirstName(expectedFirstName);

            //Assert
            expectedFirstName.Length.Should().Be(20);
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

        [Fact]
        public void setFirstName_should_thrown_exception_when_firstName_length_is_smaller_than_2()
        {
            // Arrange
            var author = CorrectAuthor();
            string expectedFirstName = "T";
            var expectedExMsg = "FirstName must contain at least 2 characters";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => author.SetFirstName(expectedFirstName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setFirstName_should_thrown_exception_when_firstName_length_is_greater_than_20()
        {
            // Arrange
            var author = CorrectAuthor();
            string expectedFirstName = "ABCDEFGHIJKLMNOPRSTUWXYZ";

            var expectedExMsg = "FirstName cannot contain more than 20 characters";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => author.SetFirstName(expectedFirstName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setLastName_should_assign_correct_lastName_to_author()
        {
            // Arrange
            var author = CorrectAuthor();
            var expectedLastName = "Kowalski";

            // Act
            author.SetLastName(expectedLastName);

            // Assert
            author.LastName.Should().BeEquivalentTo(expectedLastName);
        }

        // LastName
        [Fact]
        public void setLastName_should_thrown_exception_when_lastName_is_null()
        {
            // Arrange
            var author = CorrectAuthor();
            string expectedLastName = null;
            var expectedExMsg = $"Author with {author.AuthorId} cannot have an empty LastName";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => author.SetLastName(expectedLastName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setLastName_should_thrown_exception_when_lastName_is_whitespace()
        {
            // Arrange
            var author = CorrectAuthor();
            var expectedLastName = "";
            var expectedExMsg = $"Author with {author.AuthorId} cannot have an empty LastName";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => author.SetLastName(expectedLastName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        // TODO FIX REGEX
        [Fact]
        public void setAuthorWebsite_should_assign_correct_website_url_to_author()
        {
            // Arrange
            var author = CorrectAuthor();
            var expectedWebsiteUrl = "http://www.jonskeet.com";

            // Act
            author.SetAuthorWebsite(expectedWebsiteUrl);

            // Assert
            author.AuthorWebsite.Should().BeEquivalentTo(expectedWebsiteUrl);
        }
        
        [Fact]
        public void setAuthorWebsite_should_thrown_exception_when_website_url_doesnt_meet_required_criteria()
        {
            // Arrange
            var author = CorrectAuthor();
            var expectedWebsiteUrl = "zzz.jonskeet.com";
            var expectedExMsg = $"URL {expectedWebsiteUrl} doesn't meet required criteria";
            
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => author.SetAuthorWebsite(expectedWebsiteUrl));
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