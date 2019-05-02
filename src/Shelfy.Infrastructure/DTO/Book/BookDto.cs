using System;
using System.Collections.Generic;
using Shelfy.Infrastructure.DTO.Author;

namespace Shelfy.Infrastructure.DTO.Book
{
    public class BookDto
    {
        public Guid BookId { get; set; }
        public string Title { get;  set; }
        public string OriginalTitle { get; set; }
        public IEnumerable<AuthorFullNameDto> Authors { get; set; }
        public string Description { get;  set; }
        public string ISBN { get;  set; }
        public int Pages { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Cover { get; set; }
        public Guid UserId { get; set; }
    }
}
