using System;
using Shelfy.Core.Domain;
using Shelfy.Infrastructure.DTO.Jwt;

namespace Shelfy.Infrastructure.Services
{
    public interface IJwtHandler
    {
        JwtDto CreateToken(Guid userId, Role role);
    }
}