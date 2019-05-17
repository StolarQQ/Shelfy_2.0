using System;
using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Attributes;

namespace Shelfy.Core.Domain
{
    public class Author
    {
        private static readonly Regex ImageUrlRegex = new Regex("(http(s?):)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)");
        private static readonly Regex UrlRegex = new Regex(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$");

        [BsonId]
        public Guid AuthorId { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string FullName { get; protected set; }
        public string Description { get; protected set; }
        // Path to Author photo
        public string ImageUrl { get; protected set; }
        public DateTime? DateOfBirth { get; protected set; }
        public DateTime? DateOfDeath { get; protected set; }
        public string BirthPlace { get; protected set; }
        public string AuthorWebsite { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        // Evidence that showing the authenticity of author.
        public string AuthorSource { get; protected set; }
        // User that send request for add an author.
        public Guid UserId { get; protected set; }

        // For mongo driver
        protected Author()
        {

        }

        // TODO : UserId
        public Author(Guid authorId, string firstName, string lastName, string description, string imageUrl,
            DateTime? dateOfBirth, DateTime? dateOfDeath, string birthPlace, string authorWebsite, string authorSource, Guid userId)
        {
            AuthorId = authorId;
            SetFirstName(firstName);
            SetLastName(lastName);
            SetFullName(firstName, lastName);
            SetDescription(description);
            SetImageUrl(imageUrl);
            SetDateOfBirth(dateOfBirth);
            SetDateOfDeath(dateOfDeath);
            SetBirthPlace(birthPlace);
            SetAuthorWebsite(authorWebsite);
            SetAuthorSource(authorSource);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            UserId = userId;
        }

        public void SetFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new DomainException(ErrorCodes.InvalidFirstName, $"Author with '{AuthorId}' cannot have an empty FirstName.");
            }

            if (firstName.Length < 2)
            {
                throw new DomainException(ErrorCodes.InvalidFirstName, "FirstName must contain at least 2 characters.");

            }

            if (firstName.Length > 20)
            { 
                throw new DomainException(ErrorCodes.InvalidFirstName, "FirstName cannot contain more than 20 characters.");
            }

            FirstName = firstName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new DomainException(ErrorCodes.InvalidLastName, $"Author with '{AuthorId}' cannot have an empty LastName.");
            }

            LastName = lastName;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetFullName(string firstName, string lastName)
        {
            FullName = $"{firstName} {lastName}";
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new DomainException(ErrorCodes.InvalidDescription, "Description cannot be empty.");
            }

            if (description.Length < 15)
            {
                throw new DomainException(ErrorCodes.InvalidDescription, "Description must contain at least 15 characters.");

            }

            if (description.Length > 500)
            {
                throw new DomainException(ErrorCodes.InvalidDescription, "Description cannot contain more than 500 characters.");
            }

            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDateOfBirth(DateTime? dateOfBirth)
        {
            if (dateOfBirth > DateTime.UtcNow)
            {
                throw new DomainException(ErrorCodes.InvalidDateOfBirth, $"DateOfBirth '{dateOfBirth}' cannot be greater than '{DateTime.UtcNow}'.");
            }

            DateOfBirth = dateOfBirth;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDateOfDeath(DateTime? dateOfDeath)
        {
            if (dateOfDeath != null & dateOfDeath < DateOfBirth)
            {
                throw new DomainException(ErrorCodes.InvalidDateOfDeath, $"DateOfDeath '{dateOfDeath}' cannot be earlier than DateOfBirth '{DateOfBirth}'.");
            }

            DateOfDeath = dateOfDeath;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetBirthPlace(string birthPlace)
        {
            BirthPlace = birthPlace;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetAuthorWebsite(string authorWebsite)
        {
            if (UrlRegex.IsMatch(authorWebsite) == false)
            {
                throw new DomainException(ErrorCodes.InvalidAuthorWebsite, $"URL {authorWebsite} doesn't meet required criteria.");
            }

            AuthorWebsite = authorWebsite;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetAuthorSource(string authorSource)
        {
            if (UrlRegex.IsMatch(authorSource) == false)
            {
                throw new DomainException(ErrorCodes.InvalidAuthorSource, $"URL {authorSource} doesn't meet required criteria.");
            }

            AuthorSource = authorSource;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetImageUrl(string imageUrl)
        {
            if (ImageUrlRegex.IsMatch(imageUrl) == false)
            {
                throw new DomainException(ErrorCodes.InvalidImageUrl, $"URL {imageUrl} doesn't meet required criteria.");
            }

            ImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}