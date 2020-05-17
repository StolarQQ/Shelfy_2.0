using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shelfy.API.Framework.Extensions;
using Shelfy.Infrastructure.Commands.User;
using Shelfy.Infrastructure.Services;

namespace Shelfy.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{username}", Name = "GetUserByUserName")]
        [Authorize(Policy = "HasUserRole")]
        public async Task<IActionResult> Get(string username)
        {
            var user = await _userService.GetByUserNameAsync(username);
            if (user == null)
            {
                return NotFound($"User with username'{username}' not found");
            }

            return Ok(user);
        }

        [HttpGet(Name = "Pagination")]
        [Authorize(Policy = "HasUserRole")]
        public async Task<IActionResult> Get(int currentPage, int pageSize)
        {
            var paginatedUsers = await _userService.BrowseAsync(currentPage, pageSize);

            // Added meta data to pagination header
            Response.AddPaginationHeader(paginatedUsers.CurrentPage, paginatedUsers.PageSize,
                paginatedUsers.TotalCount, paginatedUsers.TotalPages);

            return Ok(paginatedUsers.Source);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(CreateUser command)
        {
            var userId = Guid.NewGuid();
            await _userService.RegisterAsync(userId, command.Email,
                command.Username, command.Password);

            return CreatedAtRoute("GetUserByUserName", new {command.Username }, null);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "HasAdminRole")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                NotFound($"User with id '{id}' was not found");
            }

            await _userService.DeleteAsync(user.UserId);

            return NoContent();
        }
    }
}
