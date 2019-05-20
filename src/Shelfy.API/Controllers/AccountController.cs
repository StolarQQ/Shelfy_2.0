using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.DTO.Jwt;
using Shelfy.Infrastructure.Services;

namespace Shelfy.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;

        public AccountController(IUserService userService, IMemoryCache cache)
        {
            _userService = userService;
            _cache = cache;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login command)
        {
            await _userService.LoginAsync(command.Email, command.Password);
            var jwt = _cache.Get<TokenDto>(command.Email);

            return Ok(jwt);
        }

        [HttpPost("password")]
        [Authorize(Policy = "HasUserRole")]
        public async Task<IActionResult> Post([FromBody]ChangePassword command)
        {
            await _userService.ChangePassword(UserId, command.CurrentPassword, command.NewPassword);

            return NoContent();
        }

        [HttpPost("avatar")]
        [Authorize(Policy = "HasUserRole")]
        public async Task<IActionResult> Post([FromBody]SetAvatar command)
        {
            await _userService.SetAvatar(UserId, command.AvatarUrl);

            return NoContent();
        }

        [HttpPost("deleteavatar")]
        [Authorize(Policy = "HasUserRole")]
        public async Task<IActionResult> Post()
        {
            await _userService.DeleteAvatar(UserId);

            return NoContent();
        }
    }
}