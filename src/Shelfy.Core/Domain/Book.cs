using System;
using System.Collections.Generic;

namespace Shelfy.Core.Domain
{
    public class Book
    {
        private ISet<Author> _authors = new HashSet<Author>();
        private ISet<Review> _reviews = new HashSet<Review>();

        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string ImageUrl { get; set; }
        
        public IEnumerable<Author> Authors => _authors;
        public IEnumerable<Review> Reviews => _reviews;

        protected Book()
        {
            
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new Exception($"Book with {BookId} cannot have an empty name");
            }

            Title = title;
            UpdateAt = DateTime.UtcNow;
        }
        
        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new Exception($"Book with {BookId} cannot have an empty description");
            }

            if (description.Length > 300)
            {
                throw new Exception("Description cannot be greater than 300 characters");
            }

            Description = description;
            UpdateAt = DateTime.UtcNow;
        }

        public void SetIsbn(string isbn)
        {
            if (isbn.Length != 13)
            {
                throw new Exception("ISBN number it's 13 digit format");

            }

            ISBN = isbn;
            UpdateAt = DateTime.UtcNow;
        }

        // TODO Avatar model, category
    }
}
