using System;

namespace Shelfy.Infrastructure.DTO.Author
{
    public class AuthorFullNameDto
    {
        public Guid AuthorId { get; set; }
        public string FullName { get; set; }
    }
}