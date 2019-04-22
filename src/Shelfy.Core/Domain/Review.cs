using System;

namespace Shelfy.Core.Domain
{
    public class Review
    {
        public Guid ReviewId { get; private set; }
        public int Rating { get; private set; }
        public string Comment { get; private set; }
        public Guid UserId { get; private set; }
        public Guid BookId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Review()
        {
            
        }
        
        public Review(Guid reviewId, int rating, string comment, User user, Book book)
        {
            ReviewId = reviewId;
            Rating = rating;
            Comment = comment;
            UserId = user.UserId;
            BookId = book.BookId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static Review Create(Guid reviewId, int rating, string comment, User user, Book book)
            => new Review(reviewId, rating, comment, user, book);
    }
}