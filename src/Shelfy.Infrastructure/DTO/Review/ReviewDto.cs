using System;

namespace Shelfy.Infrastructure.DTO.Review
{
    public class ReviewDto
    {
        public Guid ReviewId { get; private set; }
        public int Rating { get; private set; }
        public string Comment { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime UpdatedAt { get; private set; }
    }
}