using System;
using FluentAssertions;
using Shelfy.Core.Domain;
using Shelfy.Core.Exceptions;
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
            expectedFirstName.Length.Should().Be(2);
            _author.FirstName.Should().BeEquivalentTo(expectedFirstName);
        }

        [Fact]
        public void setFirstName_should_assign_firstName_to_author_when_firstName_length_is_20()
        {
            // Arrange
            string expectedFirstName = Helper.GenerateRandomString(20);

            // Act
            _author.SetFirstName(expectedFirstName);

            //Assert
            expectedFirstName.Length.Should().Be(20);
            _author.FirstName.Should().BeEquivalentTo(expectedFirstName);
        }

        [Fact]
        public void setFirstName_should_thrown_argException_when_firstName_is_null()
        {
            // Arrange
            string expectedFirstName = null;
            var expectedExMsg = $"Author with '{_author.AuthorId}' cannot have an empty FirstName.";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => _author.SetFirstName(expectedFirstName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setFirstName_should_thrown_argException_when_firstName_is_whitespace()
        {
            // Arrange
            var expectedFirstName = "";
            var expectedExMsg = $"Author with '{_author.AuthorId}' cannot have an empty FirstName.";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => _author.SetFirstName(expectedFirstName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setFirstName_should_thrown_argException_when_firstName_length_is_less_than_2()
        {
            // Arrange
            string expectedFirstName = "T";
            var expectedExMsg = "FirstName must contain at least 2 characters.";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => _author.SetFirstName(expectedFirstName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setFirstName_should_thrown_argException_when_firstName_length_is_greater_than_20()
        {
            // Arrange
            string expectedFirstName = Helper.GenerateRandomString(26);
            var expectedExMsg = "FirstName cannot contain more than 20 characters.";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => _author.SetFirstName(expectedFirstName));
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
        public void setLastName_should_thrown_argException_when_lastName_is_null()
        {
            // Arrange
            string expectedLastName = null;
            var expectedExMsg = $"Author with '{_author.AuthorId}' cannot have an empty LastName.";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => _author.SetLastName(expectedLastName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setLastName_should_thrown_argException_when_lastName_is_whitespace()
        {
            // Arrange
            var expectedLastName = "";
            var expectedExMsg = $"Author with '{_author.AuthorId}' cannot have an empty LastName.";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => _author.SetLastName(expectedLastName));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setDescription_should_assign_description_to_author_for_correct_input()
        {
            // Arrange
            var expectedDescription = "C# in Depth, Fourth Edition is your key to unlocking the powerful" +
                                      " new features added to the language in C# 5, 6, and 7. Following the" +
                                      " expert guidance of C# legend Jon Skeet, you'll master asynchronous functions," +
                                      " expression-bodied members, interpolated strings, tuples, and much more.";

            // Act
            _author.SetDescription(expectedDescription);

            // Assert
            _author.Description.Should().BeEquivalentTo(expectedDescription);
        }
        
        [Fact]
        public void setDescription_should_assign_description_to_author_when_description_length_is_15()
        {
            // Arrange
            var expectedDescription = Helper.GenerateRandomString(15);

            // Act
            _author.SetDescription(expectedDescription);

            // Assert
            expectedDescription.Length.Should().Be(15);
            _author.Description.Should().BeEquivalentTo(expectedDescription);
        }
        
        [Fact]
        public void setDescription_should_assign_description_to_author_when_description_length_is_500()
        {
            // Arrange
            var expectedDescription = Helper.GenerateRandomString(500);

            // Act
            _author.SetDescription(expectedDescription);

            // Assert
            expectedDescription.Length.Should().Be(500);
            _author.Description.Should().BeEquivalentTo(expectedDescription);
        }
        
        [Fact]
        public void setDescription_should_thrown_argException_when_description_is_null()
        {
            // Arrange
            string expectedDescription = null;
            var expectedExMsg = "Description cannot be empty.";

            // Act
            var ex = Assert.Throws<DomainException>(() => _author.SetDescription(expectedDescription));
            
            // Assert
            ex.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setDescription_should_thrown_argException_when_description_is_whitespace()
        {
            // Arrange
            string expectedDescription = "";
            var expectedExMsg = "Description cannot be empty.";

            // Act
            var ex = Assert.Throws<DomainException>(() => _author.SetDescription(expectedDescription));

            // Assert
            ex.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setDescription_should_thrown_argException_when_description_length_is_less_than_15()
        {
            // Arrange
            string expectedDescription = Helper.GenerateRandomString(10);
            var expectedExMsg = "Description must contain at least 15 characters.";

            // Act
            var ex = Assert.Throws<DomainException>(() => _author.SetDescription(expectedDescription));

            // Assert
            expectedDescription.Length.Should().BeLessThan(15);
            ex.Message.Should().BeEquivalentTo(expectedExMsg);
        }

        [Fact]
        public void setDescription_should_thrown_argException_when_description_length_is_greater_than_500()
        {
            // Arrange
            string expectedDescription = Helper.GenerateRandomString(2500);
            var expectedExMsg = "Description cannot contain more than 500 characters.";

            // Act
            var ex = Assert.Throws<DomainException>(() => _author.SetDescription(expectedDescription));

            // Assert
            expectedDescription.Length.Should().BeGreaterThan(500);
            ex.Message.Should().BeEquivalentTo(expectedExMsg);
        }
        
        [Fact]
        public void setDateBirth_should_assign_dateOfBirth_for_correct_DateTime()
        {
            // Arrange
            var expectedDateOfBirth = new DateTime(1950, 02, 04);

            // Act
            _author.SetDateOfBirth(expectedDateOfBirth);

            // Assert
            _author.DateOfBirth.Should().Be(expectedDateOfBirth);
        }

        [Fact]
        public void setDateBirth_should_assign_dateOfBirth_for_null()
        {
            // Arrange
            DateTime? expectedDateOfBirth = null;

            // Act
            _author.SetDateOfBirth(expectedDateOfBirth);

            // Assert
            _author.DateOfBirth.Should().Be(expectedDateOfBirth);
        }
        
        [Fact]
        public void setDateBirth_should_thrown_argException_when_dateOfBirth_is_greater_than_DateTime_now()
        {
            // Arrange
            var incorrectdDateOfBirth = new DateTime(2222,12,12);
            var exMsg = $"DateOfBirth '{incorrectdDateOfBirth}' cannot be greater than '{DateTime.UtcNow}'.";

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => _author.SetDateOfBirth(incorrectdDateOfBirth));
            ex.Message.Should().BeEquivalentTo(exMsg);
        }
        
        [Fact]
        public void setDateOfDeath_should_assign_dateOfDeath_for_correct_DateTime()
        {
            // Arrange
            var expectedDateOfDeath = new DateTime(2000, 02, 04);

            // Act
            _author.SetDateOfDeath(expectedDateOfDeath);

            // Assert
            _author.DateOfDeath.Should().Be(expectedDateOfDeath);
        }

        [Fact]
        public void setDateOfDeath_should_assign_dateOfDeath_for_null()
        {
            // Arrange
            DateTime? expectedDateOfDeath = null;

            // Act
            _author.SetDateOfDeath(expectedDateOfDeath);

            // Assert
            _author.DateOfDeath.Should().Be(expectedDateOfDeath);
        }
        
        [Fact]
        public void setDateDeath_should_thrown_argException_when_dateOfDeath_is_earlier_than_dateOfBirth()
        {
            // Arrange
            var dateOfBirth = new DateTime(1950, 12, 12);
            var incorrectdDateOfDeath = new DateTime(1850, 12, 12);
            var exMsg = $"DateOfDeath '{incorrectdDateOfDeath}' cannot be earlier than DateOfBirth '{dateOfBirth}'.";

            // Act & Assert
            _author.SetDateOfBirth(dateOfBirth);
            var ex = Assert.Throws<DomainException>(() => _author.SetDateOfDeath(incorrectdDateOfDeath));
            ex.Message.Should().BeEquivalentTo(exMsg);
        }
        
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
        public void setAuthorWebsite_should_thrown_argException_when_website_url_doesnt_meet_required_criteria()
        {
            // Arrange
            var expectedWebsiteUrl = "zzz.jonskeet.com";
            var expectedExMsg = $"URL {expectedWebsiteUrl} doesn't meet required criteria.";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => _author.SetAuthorWebsite(expectedWebsiteUrl));
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
        public void setAuthorSource_should_should_thrown_argException_when_website_url_doesnt_meet_required_criteria()
        {
            // Arrange
            var expectedAuthorSource = "badurl.wikipedia.org/wiki/C_Sharp_in_Depth";
            var expectedExMsg = $"URL {expectedAuthorSource} doesn't meet required criteria.";

            var exception = Assert.Throws<DomainException>(() => _author.SetAuthorSource(expectedAuthorSource));
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
            var expectedExMsg = $"URL {expectedAuthorImage} doesn't meet required criteria.";

            var exception = Assert.Throws<DomainException>(() => _author.SetImageUrl(expectedAuthorImage));
            exception.Message.Should().BeEquivalentTo(expectedExMsg);
        }
    }
}