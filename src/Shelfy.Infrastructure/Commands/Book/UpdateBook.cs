using System;
using System.Collections.Generic;

namespace Shelfy.Infrastructure.Commands.Book
{
    public class UpdateBook
    {
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public IEnumerable<Guid> AuthorsIds { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public int Pages { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Cover { get; set; }
    }
}
