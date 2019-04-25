using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.Services;

namespace Shelfy.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("{id}", Name = "GetBookById")]
        public async Task<IActionResult> Get(Guid id)
        {
            var book = await _bookService.GetAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }


        [HttpGet("{isbn}", Name = "GetBookByIsbn")]
        public async Task<IActionResult> Get(string isbn)
        {
            var book = await _bookService.GetAsync(isbn);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateBook book)
        {
            await _bookService.AddAsync(book.Title, book.OriginalTitle,
                book.Description, book.ISBN, book.Pages, book.Publisher, book.PublishedAt, book.AuthorsId);

            return Created("GetBookById", null);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id,
            [FromBody] JsonPatchDocument<UpdateBook> patchBook)
        {
            await _bookService.UpdateAsync(id, patchBook);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookService.DeleteAsync(id);

            return NoContent();
        }
    }
}