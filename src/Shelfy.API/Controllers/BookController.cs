using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.Services;

namespace Shelfy.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ApiControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("{id}", Name = "GetBookById")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            var book = await _bookService.GetAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpGet("isbn/{isbn}", Name = "GetBookByIsbn")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string isbn)
        {
            var book = await _bookService.GetAsync(isbn);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        //[HttpGet(Name = "BrowseAsync")]
        //public async Task<IActionResult> Browse()
        //{
        //    var book = await _bookService.BrowseAsync();
         
        //    return Ok(book);
        //}
        
        [HttpPost]
        [Authorize(Policy = "HasUserRole")]
        public async Task<IActionResult> Post([FromBody]CreateBook book)
        {
            await _bookService.AddAsync(book.Title, book.OriginalTitle,
                book.Description, book.ISBN, book.Cover, book.Pages, book.Publisher, book.PublishedAt, book.AuthorsId, UserId);

            return CreatedAtRoute("GetBookByIsbn", new { Isbn = book.ISBN}, book);
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "HasModeratorRole")]
        public async Task<IActionResult> Patch(Guid id,
            [FromBody] JsonPatchDocument<UpdateBook> patchBook)
        {
            await _bookService.UpdateAsync(id, patchBook);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "HasAdminRole")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookService.DeleteAsync(id);

            return NoContent();
        }
    }
}