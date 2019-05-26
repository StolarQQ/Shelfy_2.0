using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shelfy.API.Framework.Extensions;
using Shelfy.Infrastructure.Commands.Book;
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

        [HttpGet(Name = "GetAllAsync")]
        public async Task<IActionResult> Get(int currentPage, int pageSize, string query)
        {
            var paginatedBook = await _bookService.BrowseAsync(currentPage, pageSize, query);

            Response.AddPaginationHeader(paginatedBook.CurrentPage, paginatedBook.PageSize,
                paginatedBook.TotalCount, paginatedBook.TotalPages, query);

            return Ok(paginatedBook.Source);
        }

        [HttpPost]
        [Authorize(Policy = "HasUserRole")]
        public async Task<IActionResult> Post([FromBody]CreateBook book)
        {
            var bookId = Guid.NewGuid();
            await _bookService.AddAsync(bookId, book.Title, book.OriginalTitle,
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