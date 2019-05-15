using System;
using FluentAssertions;
using Shelfy.Core.Domain;
using Xunit;

namespace Shelfy.Tests.Domain
{
    public class AuthorDomainTests
    {
        private readonly Author _author;

        public AuthorDomainTests()
        {
            var authorId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var defaultAuthorImage = "https://www.stolarstate.pl/avatar/author/default.png";
            var authorWebsite = "https://stackoverflow.com/users/22656/jon-skeet";
            _author = new Author(authorId, "Jon", "Skeet", "C# in Depth Author", defaultAuthorImage,
                new DateTime(1984, 01, 01), null, "Texas", authorWebsite, authorWebsite, userId);
        }

        [Fact]
        public void setFirstName_should_assign_firstName_to_author_for_correct_input()
        {
            // Arrange
            var expectedFirstName = "Micheal";

            // Act
            _author.SetFirstName(expectedFirstName);

            // Assert
            _author.FirstName.Should().BeEquivalentTo(expectedFirstName);
        }

        [Fact]
        public void setFirstName_should_assign_firstName_to_author_when_firstName_length_is_2()
        {
            // Arrange
            string expectedFirstName = "TX";

            // Act
            _author.SetFirstName(expectedFirstName);

            //Assert
            _author.FirstName.Should().BeEquivalentTo(expectedFirstName);
            expectedFirstName.Length.Should().Be(2);
        }

        [Fact]
        public void setFirstName_should_assign_firstName_to_author_when_firstName_length_is_20()
        {
            // Arrange
            string expectedFirstName = "ABCDEFGHIJKLMNOPRSTU";

            // Act
            _author.SetFirstName(expectedFirstName);

            //Assert
            expectedFirstName.Length.Should().Be(20);
            _author.FirstName.Should().BeEquivalentTo(expectedFirstName);
        }

        [Fact]
        public void setFirstName_should_thrown_exception_when_firstName_is_null()
        {
            // Arrange
            string expectedFirstName = null;
            var expectedExMsg = $"Author with {_author.AuthorId} cannot have an empty FirstName";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _author.SetFirstName(expectedFirstName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setFirstName_should_thrown_exception_when_firstName_is_whitespace()
        {
            // Arrange
            var expectedFirstName = "";
            var expectedExMsg = $"Author with {_author.AuthorId} cannot have an empty FirstName";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _author.SetFirstName(expectedFirstName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setFirstName_should_thrown_exception_when_firstName_length_is_smaller_than_2()
        {
            // Arrange
            string expectedFirstName = "T";
            var expectedExMsg = "FirstName must contain at least 2 characters";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _author.SetFirstName(expectedFirstName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setFirstName_should_thrown_exception_when_firstName_length_is_greater_than_20()
        {
            // Arrange
            string expectedFirstName = "ABCDEFGHIJKLMNOPRSTUWXYZ";
            var expectedExMsg = "FirstName cannot contain more than 20 characters";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _author.SetFirstName(expectedFirstName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setLastName_should_assign_lastName_to_author_for_correct_input()
        {
            // Arrange
            var expectedLastName = "Kowalski";

            // Act
            _author.SetLastName(expectedLastName);

            // Assert
            _author.LastName.Should().BeEquivalentTo(expectedLastName);
        }

        // LastName
        [Fact]
        public void setLastName_should_thrown_exception_when_lastName_is_null()
        {
            // Arrange
            string expectedLastName = null;
            var expectedExMsg = $"Author with {_author.AuthorId} cannot have an empty LastName";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _author.SetLastName(expectedLastName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setLastName_should_thrown_exception_when_lastName_is_whitespace()
        {
            // Arrange
            var expectedLastName = "";
            var expectedExMsg = $"Author with {_author.AuthorId} cannot have an empty LastName";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _author.SetLastName(expectedLastName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        // TODO FIX REGEX
        [Fact]
        public void setAuthorWebsite_should_assign_correct_website_url_to_author()
        {
            // Arrange
            var expectedWebsiteUrl = "http://www.jonskeet.com";

            // Act
            _author.SetAuthorWebsite(expectedWebsiteUrl);

            // Assert
            _author.AuthorWebsite.Should().BeEquivalentTo(expectedWebsiteUrl);
        }

        [Fact]
        public void setAuthorWebsite_should_thrown_exception_when_website_url_doesnt_meet_required_criteria()
        {
            // Arrange
            var expectedWebsiteUrl = "zzz.jonskeet.com";
            var expectedExMsg = $"URL {expectedWebsiteUrl} doesn't meet required criteria";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _author.SetAuthorWebsite(expectedWebsiteUrl));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setAuthorSource_should_assign_correct_source_url_to_author()
        {
            // Arrange
            var expectedAuthorSource = "https://en.wikipedia.org/wiki/C_Sharp_in_Depth";

            // Act
            _author.SetAuthorSource(expectedAuthorSource);

            // Assert
            _author.AuthorSource.Should().BeEquivalentTo(expectedAuthorSource);
        }

        [Fact]
        public void setAuthorSource_should_should_thrown_exception_when_website_url_doesnt_meet_required_criteria()
        {
            // Arrange
            var expectedAuthorSource = "badurl.wikipedia.org/wiki/C_Sharp_in_Depth";
            var expectedExMsg = $"URL {expectedAuthorSource} doesn't meet required criteria";

            var exception = Assert.Throws<ArgumentException>(() => _author.SetAuthorSource(expectedAuthorSource));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setImageUrl_should_assign_correct_image_url_to_author()
        {
            // Arrange
            var expectedAuthorImage = "https://www.avatar.com/avatar.png";

            // Act
            _author.SetImageUrl(expectedAuthorImage);

            // Assert
            _author.ImageUrl.Should().BeEquivalentTo(expectedAuthorImage);
        }

        [Fact]
        public void setImageUrl_should_thrown_exception_when_image_url_doesnt_meet_required_criteria()
        {
            // Arrange
            var expectedAuthorImage = "https://www.avatar.com/avatar.badextension";
            var expectedExMsg = $"URL {expectedAuthorImage} doesn't meet required criteria";

            var exception = Assert.Throws<ArgumentException>(() => _author.SetImageUrl(expectedAuthorImage));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }
    }
}