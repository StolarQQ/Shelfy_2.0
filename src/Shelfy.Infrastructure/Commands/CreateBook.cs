using System;
using System.Collections.Generic;

namespace Shelfy.Infrastructure.Commands
{
    public class CreateBook
    {
       public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public int Pages { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishedAt { get; set; }
        public IEnumerable<Guid> AuthorsId { get; set;}
    }
}