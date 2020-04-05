using System;
using System.Collections.Generic;
using Shelfy.Infrastructure.DTO.Author;
using Shelfy.Infrastructure.DTO.Review;

namespace Shelfy.Infrastructure.DTO.Book
{
    public class BookDetailsDto
    {
        public Guid BookId { get; set; }
        public string BookUrl { get; set; }
        public string Title { get; set; }
        public IEnumerable<AuthorShortcutDto> Authors { get; set; }
        public string Description { get; set; }
        public DateTime PublishedAt { get; set; }
        public IEnumerable<ReviewDto> Reviews { get; set; }
        public int ReviewCount { get; set; }
        public double Rating { get; set; }
        public string Cover { get; set; }
        public string OriginalTitle { get; set; }
        public string ISBN { get; set; }
        public int Pages { get; set; }
        public string Publisher { get; set; }
        public Guid UserId { get; set; }
    }
}