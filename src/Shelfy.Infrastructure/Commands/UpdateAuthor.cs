using System;

namespace Shelfy.Infrastructure.Commands
{
    public class UpdateAuthor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string BirthPlace { get; set; }
        public string AuthorWebsite { get; set; }
        public string AuthorSource { get; set; }
    }
}
