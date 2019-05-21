using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shelfy.API.Framework.Extensions;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.Services;

namespace Shelfy.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IReviewService _reviewService;
    
        public UserController(IUserService userService, IReviewService reviewService)
        {
            _userService = userService;
            _reviewService = reviewService;
        }

        [HttpGet("{id}", Name = "GetUserById")]
        [Authorize(Policy = "HasUserRole")]
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

        [HttpGet("review", Name = "GetReviewsForUser")]
        [Authorize(Policy = "HasUserRole")]
        public async Task<IActionResult> Get()
        {
            var userReviews = await _reviewService.GetReviewsForUserAsync(UserId);
            
            return Ok(userReviews);
        }

        [HttpGet(Name = "Pagination")]
        [Authorize(Policy = "HasUserRole")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int currentPage, int pageSize)
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

            return CreatedAtRoute("GetUserById", new {Id = userId}, null);
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
