using System;

namespace Shelfy.Infrastructure.DTO.Review
{
    public class ReviewDto
    {
        public Guid ReviewId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public Guid UserId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}