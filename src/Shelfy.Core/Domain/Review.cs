using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Shelfy.Core.Domain
{
    public class Review
    {
        [BsonId]
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
        
        public Review(Guid reviewId, int rating, string comment, Guid userId, Guid bookId)
        {
            ReviewId = reviewId;
            SetRating(rating);
            SetComment(comment);
            UserId = userId;
            BookId = bookId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetRating(int rating)
        {
            if (rating < 1)
            {
                throw new DomainException(ErrorCodes.InvalidRating, "Rating cannot be lower than 1");
            }
            if (rating > 6)
            {
                throw new DomainException(ErrorCodes.InvalidRating, "Rating cannot be greater than 6");
            }

            Rating = rating;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetComment(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new DomainException(ErrorCodes.InvalidComment, "Comment cannot be empty");
            }

            if (comment.Length < 5)
            {
                throw new DomainException(ErrorCodes.InvalidComment, "Comment must contain at least 5 characters");
            }

            if (comment.Length > 500)
            {
                throw new DomainException(ErrorCodes.InvalidComment, "Comment cannot contain more than 500 characters");
            }

            Comment = comment;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public static Review Create(Guid reviewId, int rating, string comment, Guid userId, Guid bookId)
            => new Review(reviewId, rating, comment, userId, bookId);
    }
}