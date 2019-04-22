using System;
using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Attributes;

namespace Shelfy.Core.Domain
{
    public class Author
    {
        private static readonly Regex ImageUrlRegex = new Regex("(http(s?):)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)");

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
        //public Guid UserId { get; protected set; }

        // For mongo driver
        protected Author()
        {
            
        }
        // TODO Avatar, UserId
        public Author(Guid authorId, string firstName, string lastName, string description, string imageUrl,
            DateTime? dateOfBirth, DateTime? dateOfDeath, string birthPlace, string authorWebsite, string authorSource)
        {
            AuthorId = authorId;
            SetFirstName(firstName);
            SetLastName(lastName);
            SetDescription(description);
            FullName = FirstName + " " + LastName;
            ImageUrl = imageUrl;
            SetDateOfBirth(dateOfBirth);
            SetDateOfDeath(dateOfDeath);
            SetBirthPlace(birthPlace);
            SetAuthorWebiste(authorWebsite);
            SetAuthorSource(authorSource);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            //UserId = user.UserId;
        }

        private void SetAuthorSource(string authorSource)
        {
            if (Uri.IsWellFormedUriString(authorSource, UriKind.RelativeOrAbsolute) == false)
            {
                throw new ArgumentException($"URL {authorSource} doesn't meet required criteria");
            }

            AuthorSource = authorSource;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetAuthorWebiste(string authorWebsite)
        {
            if (Uri.IsWellFormedUriString(authorWebsite, UriKind.RelativeOrAbsolute) == false)
            {
                throw new ArgumentException($"URL {authorWebsite} doesn't meet required criteria");
            }

            AuthorWebsite = authorWebsite;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetBirthPlace(string birthPlace)
        {
            BirthPlace = birthPlace;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetDateOfBirth(DateTime? dateOfBirth)
        {
            DateOfBirth = dateOfBirth;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetDateOfDeath(DateTime? dateOfDeath)
        {
            DateOfDeath = dateOfDeath;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetImageUrl(string imageUrl)
        {
            if (ImageUrlRegex.IsMatch(imageUrl) == false)
            {
                throw new ArgumentException($"URL {imageUrl} doesn't meet required criteria");
            }

            ImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be empty");
            }

            if (description.Length < 15)
            {
                throw new ArgumentException("Description must contain at least 15 characters");

            }

            if (description.Length > 500)
            {
                throw new ArgumentException("Description cannot contain more than 300 characters");
            }

            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetLastName(string lastName)
        {
            LastName = lastName;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetFirstName(string firstName)
        {
            FirstName = firstName;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}