using Shelfy.Core.Domain;

namespace Shelfy.Infrastructure.DTO.Jwt
{
    public class TokenDto
    {
        public string Token { get; set; }
        public Role Role { get; set; }
        public long Expires { get; set; }
    }
}