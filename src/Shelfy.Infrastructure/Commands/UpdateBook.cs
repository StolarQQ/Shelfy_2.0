using System;

namespace Shelfy.Infrastructure.Commands
{
    public class UpdateBook
    {
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public int Pages { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Cover { get; set; }
    }
}
