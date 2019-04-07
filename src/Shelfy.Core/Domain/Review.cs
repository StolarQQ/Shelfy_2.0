using System;

namespace Shelfy.Core.Domain
{
    public class Review
    {
        public Guid ReviewId { get; protected set; }
        public int Rating { get; protected set; }
        public string Comment { get; protected set; }
        public Guid UserId { get; protected set; }
        public Guid BookId { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        
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
    }
}