using System;

namespace Shelfy.Infrastructure.DTO.Author
{
    public class AuthorNameDto
    {
        public Guid AuthorId { get; set; }
        public string ProfileUrl { get; set; }
    
        public string FullName { get; set; }
    }
}