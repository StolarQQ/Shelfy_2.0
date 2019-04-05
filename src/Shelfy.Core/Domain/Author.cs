using System;
using System.Text.RegularExpressions;

namespace Shelfy.Core.Domain
{
    public class Author
    {
        private static readonly Regex ImageUrlRegex = new Regex("(http(s?):)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)");
        
        public Guid AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        // Path to Author photo
        public string ImageUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string BirthPlace { get; set; }
        public string AuthorWebsite { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Evidence that showing the authenticity of author.
        public string AuthorSource { get; set; }
        // User that send request for add an author.
        public Guid UserId { get; set; }

        // EF mapping
        protected Author()
        {
            
        }

        public Author(Guid authorId, string firstName, string lastName, string description, string imageUrl,
            DateTime dateOfBirth, DateTime dateOfDeath, string birthPlace, string authorWebsite, string authorSource, User user)
        {
            AuthorId = authorId;
            SetFirstName(firstName);
            SetLastName(lastName);
            SetDescription(description);
            SetImageUrl(imageUrl);
            SetDateOfBrith(dateOfBirth);
            SetDateOfDeath(dateOfDeath);
            SetBirthPlace(birthPlace);
            SetAuthorWebiste(authorWebsite);
            SetAuthorSource(authorSource);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            UserId = user.UserId;

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
            throw new NotImplementedException();
        }

        private void SetDateOfDeath(DateTime dateOfDeath)
        {
            throw new NotImplementedException();
        }

        private void SetDateOfBrith(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
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

            if (description.Length > 300)
            {
                throw new ArgumentException("Description cannot contain more than 300 characters");
            }

            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        private void SetFirstName(string firstName)
        {
            throw new NotImplementedException();
        }
    }
}