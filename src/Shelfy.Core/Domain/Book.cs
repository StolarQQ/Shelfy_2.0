using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Shelfy.Core.Domain
{
    public class Book
    {
        private static readonly Regex UrlRegex = new Regex("(http(s?):)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)");

        private ISet<Author> _authors = new HashSet<Author>();
        private ISet<Review> _reviews = new HashSet<Review>();

        public Guid BookId { get; protected set; }
        public string Title { get; protected set; }
        public string OriginalTitle { get; protected set; }
        public string Description { get; protected set; }
        public string ISBN { get; protected set; }
        public int Pages { get; protected set; }
        public string Publisher { get; protected set; }
        public DateTime PublishedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        public string ImageUrl { get; protected set; }


        public IEnumerable<Author> Authors => _authors;
        public IEnumerable<Review> Reviews => _reviews;

        // For entity framework mapping
        protected Book()
        {

        }

        public Book(Guid bookId, string title, string originalTitle,
            string description, string isbn, int pages, string publisher, string imageUrl)
        {
            BookId = bookId;
            SetTitle(title);
            OriginalTitle = originalTitle;
            SetDescription(description);
            SetIsbn(isbn);
            SetPages(pages);
            SetPublisher(publisher);
            PublishedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            SetImageUrl(imageUrl);
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
            
            if (description.Length > 300)
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
            if (UrlRegex.IsMatch(imageUrl) == false)
            {
                throw new ArgumentException($"URL {imageUrl} doesn't meet required criteria");
            }

            ImageUrl = imageUrl;
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

        public void AddAuthor(Author author)
        {
            var authorExist = Authors.SingleOrDefault(x => x.AuthorId == author.AuthorId);
            if (authorExist != null)
            {
                throw new ArgumentException($"Author with id: '{author.AuthorId}' already added for Book: '{Title}'. ");
            }

            _authors.Add(author);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveAuthor(Author author)
        {
            var authorExist = Authors.SingleOrDefault(x => x.AuthorId == author.AuthorId);
            if (authorExist == null)
            {
                throw new ArgumentException($"Author with id: '{author.AuthorId}' was not found for Book: '{Title}'. ");
            }

            _authors.Remove(author);
            UpdatedAt = DateTime.UtcNow;
        }
        
        // TODO AddReview, DeleteReview, Update?,
    }
}
