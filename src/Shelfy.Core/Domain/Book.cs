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
        private ISet<Guid> _authorsIds = new HashSet<Guid>();
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
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        // Path to book cover
        public string Cover { get; protected set; }
        public string BookUrl => $"https://www.mysite.com/book/{BookId}";
        //User that added book
        public Guid UserId { get; protected set; }

        public int ReviewCount => _reviews.Count;
        public double Rating => ReviewCount == 0 ? 0 : Math.Round(_reviews.Average(x => x.Rating), 2);

        public IEnumerable<Guid> AuthorsIds => _authorsIds;
        public IEnumerable<Review> Reviews => _reviews;

        // For mongo driver 
        protected Book()
        {

        }

        public Book(string title, string originalTitle,
            string description, string isbn, string cover,
            int pages, string publisher, DateTime publishedAt, Guid userId)
        {
            BookId = Guid.NewGuid();
            SetTitle(title);
            OriginalTitle = originalTitle;
            SetDescription(description);
            SetIsbn(isbn);
            SetCover(cover);
            SetPages(pages);
            SetPublisher(publisher);
            PublishedAt = publishedAt;
            UpdatedAt = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            UserId = userId;
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException($"Book with {BookId} cannot have an empty title.");
            }

            Title = title;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be empty.");
            }

            if (description.Length < 15)
            {
                throw new ArgumentException("Description must contain at least 15 characters.");
            }

            if (description.Length > 500)
            {
                throw new ArgumentException("Description cannot contain more than 500 characters.");
            }

            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetIsbn(string isbn)
        {
            if (isbn.Length != 13)
            {
                throw new ArgumentException("ISBN number must contain exactly 13 characters.");
            }

            ISBN = isbn;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPages(int pages)
        {
            if (pages <= 0)
            {
                throw new ArgumentException("Number of pages cannot be zero or negative.");
            }

            Pages = pages;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetCover(string cover)
        {
            if (CoverRegex.IsMatch(cover) == false)
            {
                throw new ArgumentException($"Cover URL {cover} doesn't meet required criteria.");
            }

            Cover = cover;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPublisher(string publisher)
        {
            if (string.IsNullOrWhiteSpace(publisher))
            {
                throw new ArgumentException("Publisher cannot be empty.");
            }

            Publisher = publisher;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddAuthor(Guid authorId)
        {
            var authorExist = AuthorsIds.SingleOrDefault(x => x == authorId);
            if (authorExist != Guid.Empty)
            {
                throw new ArgumentException($"Author with id: '{authorId}' already added for Book: '{Title}'.");
            }

            _authorsIds.Add(authorId);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveAuthor(Guid authorId)
        {
            var authorExist = AuthorsIds.SingleOrDefault(x => x == authorId);
            if (authorExist == null)
            {
                throw new ArgumentException($"Author with id: '{authorId}' was not found for Book: '{Title}'.");
            }

            _authorsIds.Remove(authorId);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddReview(Review review)
        {
            var reviewExist = Reviews.SingleOrDefault(x => x.UserId == review.UserId);
            if (reviewExist != null)
            {
                throw new ArgumentException($"User with id '{review.UserId}' already added review for book {Title}.");
            }

            _reviews.Add(review);
            UpdatedAt = DateTime.UtcNow;
        }

        public void DeleteReview(Guid userId)
        {
            var reviewExist = Reviews.SingleOrDefault(x => x.UserId == userId);
            if (reviewExist == null)
            {
                throw new ArgumentException($"Review by user with id '{userId}' not exist for book {Title}.");
            }

            _reviews.Remove(reviewExist);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
