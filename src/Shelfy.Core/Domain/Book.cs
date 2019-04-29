using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Attributes;

namespace Shelfy.Core.Domain
{
    public class Book
    {
        private static readonly Regex CoverRegex = new Regex("(http(s?):)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)");

        [BsonElement]
        private ISet<Guid> _authors = new HashSet<Guid>();
        [BsonElement]
        private ISet<Review> _reviews = new HashSet<Review>();

        [BsonId]
        public Guid BookId { get; protected set; }
        public string Title { get; protected set; }
        public string OriginalTitle { get; protected set; }
        public string Description { get; protected set; }
        public string ISBN { get; protected set; }
        public int Pages { get; protected set; }
        public string Publisher { get; protected set; }
        public DateTime PublishedAt { get; protected set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; protected set; }
        // Path to book cover
        public string Cover { get; protected set; }

        public IEnumerable<Guid> Authors => _authors;
        public IEnumerable<Review> Reviews => _reviews;

        public int ReviewCount => _reviews.Count;
        public double Rating => _reviews.Average(x => x.Rating);

        // For mongo driver 
        protected Book()
        {

        }

        public Book(string title, string originalTitle,
            string description, string isbn, int pages, string publisher, DateTime publishedAt)
        {
            BookId = Guid.NewGuid();
            SetTitle(title);
            OriginalTitle = originalTitle;
            SetDescription(description);
            SetIsbn(isbn);
            SetPages(pages);
            SetPublisher(publisher);
            PublishedAt = publishedAt;
            UpdatedAt = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException($"Book with {BookId} cannot have an empty title");
            }

            Title = title;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDescription(string description)
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

        public void SetIsbn(string isbn)
        {
            if (isbn.Length != 13)
            {
                throw new ArgumentException("ISBN number must contain exactly 13 characters");
            }

            ISBN = isbn;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPages(int pages)
        {
            if (pages <= 0)
            {
                throw new ArgumentException("Number of pages cannot be zero or negative");
            }

            Pages = pages;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void SetImageUrl(string imageUrl)
        {
            if (CoverRegex.IsMatch(imageUrl) == false)
            {
                throw new ArgumentException($"URL {imageUrl} doesn't meet required criteria");
            }

            Cover = imageUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPublisher(string publisher)
        {
            if (string.IsNullOrWhiteSpace(publisher))
            {
                throw new ArgumentException("Publisher cannot be empty");
            }

            Publisher = publisher;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddAuthor(Guid authorId)
        {
            var authorExist = Authors.SingleOrDefault(x => x == authorId);
            if (authorExist != Guid.Empty)
            {
                throw new ArgumentException($"Author with id: '{authorId}' already added for Book: '{Title}'. ");
            }

            _authors.Add(authorId);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveAuthor(Guid authorId)
        {
            var authorExist = Authors.SingleOrDefault(x => x == authorId);
            if (authorExist == null)
            {
                throw new ArgumentException($"Author with id: '{authorId}' was not found for Book: '{Title}'. ");
            }

            _authors.Remove(authorId);
            UpdatedAt = DateTime.UtcNow;
        }

        // TODO AddReview, DeleteReview, Update?,
    }
}
