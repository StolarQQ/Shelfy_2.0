using System;

namespace Shelfy.Infrastructure.Commands
{
    public class CreateBookReview
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
