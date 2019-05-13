using System;

namespace Shelfy.Infrastructure.Commands
{
    public class DeleteBookReview
    {
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
    }
}