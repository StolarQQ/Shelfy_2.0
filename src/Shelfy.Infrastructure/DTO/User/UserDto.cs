using System;

namespace Shelfy.Infrastructure.DTO.User
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}