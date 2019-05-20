using System;

namespace Shelfy.Infrastructure.DTO.Author
{
    public class AuthorDto
    {
        public Guid AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string BirthPlace { get; set; }
        public string AuthorWebsite { get; set; }
        public string AuthorSource { get; set; }
        public string ProfileUrl { get; set; }
        public Guid UserId { get; set; }
    }
}