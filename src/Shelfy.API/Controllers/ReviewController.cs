using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.Services;

namespace Shelfy.API.Controllers
{
    [Route("book/{bookId}/[controller]")]
    [ApiController]
    public class ReviewController : ApiControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [Authorize(Policy = "HasUserRole")]
        public async Task<IActionResult> Post([FromBody]CreateBookReview command)
        {
            await _reviewService.AddAsync(command.Rating, command.Comment, UserId, command.BookId);

            return Created("", command);
        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = "HasUserRole")]
        public async Task<IActionResult> Post([FromBody] Guid bookId)
        {
            await _reviewService.DeleteAsync(bookId, UserId);

            return NoContent();
        }
    }
}