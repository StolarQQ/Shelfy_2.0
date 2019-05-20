using System;
using System.Collections.Generic;
using Shelfy.Infrastructure.DTO.Author;

namespace Shelfy.Infrastructure.DTO.Book
{
    public class BookDto
    {
        public Guid BookId { get; set; }
        public string Title { get;  set; }
        public IEnumerable<AuthorNameDto> Authors { get; set; }
        public string Description { get;  set; }
        public DateTime PublishedAt { get; set; }
        public int ReviewCount { get; set; }
        public double Rating { get; set; }
        public string Cover { get; set; }
        public string BookUrl { get; set; }
    }
}
