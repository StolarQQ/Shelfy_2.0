using System;

namespace Shelfy.Infrastructure.Commands
{
    public class CreateBookReview
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
