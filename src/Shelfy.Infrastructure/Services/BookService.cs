using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.DTO.Author;
using Shelfy.Infrastructure.DTO.Book;
using Shelfy.Infrastructure.Extensions;

namespace Shelfy.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BookDto> GetAsync(Guid id)
        {
            var book = await _bookRepository.GetOrFailAsync(id);

            var authors = await GetAuthors(book.AuthorsIds);
            var bookDto = SetUpBook(book, authors);

            return bookDto;
        }

        public async Task<BookDto> GetAsync(string isbn)
        {
            var book = await _bookRepository.GetOrFailAsync(isbn);

            var authors = await GetAuthors(book.AuthorsIds);
            var bookDto = SetUpBook(book, authors);

            return bookDto;
        }

        // TODO This method will be replace by pagination one
        public async Task<IEnumerable<BookDto>> BrowseAsync()
        {
            _logger.LogInformation("Fetching Data from Book repository");
            var books = await _bookRepository.BrowseAsync();

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task AddAsync(string title, string originalTitle,
            string description, string isbn, string cover, int pages, string publisher,
            DateTime publishedAt, IEnumerable<Guid> authorsId, Guid userId)
        {
            var book = await _bookRepository.GetByIsbnAsync(isbn);
            if (book != null)
            {
                throw new Exception($"Book with ISBN {isbn} already exist");
            }

            var validCover = cover.DefaultBookCoverValidation();
            book = new Book(title, originalTitle, description, isbn, validCover, pages, publisher, publishedAt, userId);

            foreach (var id in authorsId)
            {
                var author = await _authorRepository.GetByIdAsync(id);
                if (author == null)
                {
                    throw new Exception($"Author with id '{id} not exist");
                }

                book.AddAuthor(id);
            }

            await _bookRepository.AddAsync(book);
            _logger.LogInformation($"Book '{book.Title}' with id '{book.BookId} was created at '{book.CreatedAt}'");
        }

        public async Task UpdateAsync(Guid id, JsonPatchDocument<UpdateBook> patchBook)
        {
            var bookToUpdate = await _bookRepository.GetOrFailAsync(id);

            var book = _mapper.Map<UpdateBook>(bookToUpdate);
            patchBook.ApplyTo(book);

            // Mapping changes to bookToUpdate
            bookToUpdate = _mapper.Map(book, bookToUpdate);

            if (bookToUpdate.IsValid())
            {
                await _bookRepository.UpdateAsync(bookToUpdate);
                _logger.LogInformation($"Book '{bookToUpdate.Title}' with id '{id} was updated at {DateTime.UtcNow}'");
            }
            else
            {
                _logger.LogWarning($"Error occured while updating book '{bookToUpdate.Title}' with id '{id}' model is not valid");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _bookRepository.GetOrFailAsync(id);

            await _bookRepository.RemoveAsync(book.BookId);
            _logger.LogInformation($"Book '{book.Title}' with id '{id}' was deleted at {DateTime.UtcNow}'");
        }

        /// <summary>
        /// Get authors by link IDs in book. 
        /// </summary>
        /// <param name="authorsId"></param>
        /// <returns>List of authors</returns>
        private async Task<IEnumerable<Author>> GetAuthors(IEnumerable<Guid> authorsId)
        {
            var authors = new List<Author>();

            foreach (var guid in authorsId)
            {
                var author = await _authorRepository.GetByIdAsync(guid);
                authors.Add(author);
            }

            return authors;
        }

        /// <summary>
        /// Set up bookDto, assign name of authors
        /// </summary>
        /// <param name="book"></param>
        /// <param name="authors"></param>
        /// <returns></returns>
        private BookDto SetUpBook(Book book, IEnumerable<Author> authors)
        {
            var bookDto = _mapper.Map<BookDto>(book);
            var authorsDto = _mapper.Map<IEnumerable<AuthorFullNameDto>>(authors);
            bookDto.Authors = authorsDto;

            return bookDto;
        }
    }

}