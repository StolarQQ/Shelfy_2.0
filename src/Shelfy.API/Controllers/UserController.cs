using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.Services;

namespace Shelfy.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with id '{id}' not found");
            }

            return Ok(user);
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var user = await _userService.GetByUserNameAsync(username);
            if (user == null)
            {
                return NotFound($"User with id '{username}' not found");
            }

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userService.GetAllAsync();
           
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateUser createUser)
        {
            var userId = Guid.NewGuid();
            await _userService.RegisterAsync(userId, createUser.Email,
                createUser.Username, createUser.Password);

            return CreatedAtRoute("GetUserById", new {Id = userId}, createUser);
        }
    }
}
