using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.Services;

namespace Shelfy.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(Guid id)
        {
            var author = await _authorService.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound($"Author with id '{id}' was not found");
            }

            return Ok(author);
        }

        [HttpGet(Name = "BrowseByPhraseAsync")]
        public async Task<IActionResult> Get(string phrase)
        {
            if (phrase == null)
            {
                return BadRequest();
            }

            var authors = await _authorService.BrowseByPhraseAsync(phrase);

            if (authors == null)
            {
                return NotFound($"Authors with phrase '{phrase}' was not found");
            }

            return Ok(authors);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateAuthor author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            var authorId = Guid.NewGuid();

            await _authorService.RegisterAsync(authorId, author.FirstName, author.LastName, author.Description,
                author.ImageUrl, author.DateOfBirth, author.DateOfDeath, author.BirthPlace,
                author.AuthorWebsite, author.AuthorSource);

            return CreatedAtRoute("Get", new { id = authorId }, $"Author with id : '{authorId}' was created");
        }
    }
}