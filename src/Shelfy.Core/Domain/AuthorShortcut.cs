using System;

namespace Shelfy.Core.Domain
{
    // Redundant information for browsing books
    public class AuthorShortcut
    {
        public Guid AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileUrl { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}