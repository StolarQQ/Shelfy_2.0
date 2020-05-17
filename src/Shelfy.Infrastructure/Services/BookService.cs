using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Shelfy.Core.Domain;
using Shelfy.Core.Exceptions;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.Commands.Book;
using Shelfy.Infrastructure.DTO.Book;
using Shelfy.Infrastructure.Exceptions;
using Shelfy.Infrastructure.Extensions;
using Shelfy.Infrastructure.Pagination;
using ErrorCodes = Shelfy.Infrastructure.Exceptions.ErrorCodes;

namespace Shelfy.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;
        private readonly IValidator<Book> _bookValidator;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository,
            IMapper mapper, ILogger<BookService> logger, IValidator<Book> bookValidator)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
            _bookValidator = bookValidator;
        }

        public async Task<BookDetailsDto> GetByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetOrFailAsync(id);

            return _mapper.Map<BookDetailsDto>(book);
        }

        public async Task<BookDetailsDto> GetByIsbnAsync(string isbn)
        {
            var book = await _bookRepository.GetOrFailAsync(isbn);
            
            return _mapper.Map<BookDetailsDto>(book);
        }

        public async Task<PagedResult<BookDto>> BrowseAsync(int currentPage, int pageSize, string query)
        {
            _logger.LogInformation("Fetching Data from Book repository");
            var books = await _bookRepository.BrowseAsync(currentPage, pageSize, query);

            return _mapper.Map<PagedResult<BookDto>>(books);

        }

        // Testing query performance 
        public async Task<IEnumerable<BookDto>> GetAllBooks()
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Fetching Data from Book repository");
            var books = await _bookRepository.GetAllBooks();
            var bookDto = _mapper.Map<IEnumerable<BookDto>>(books);

            stopwatch.Stop();
            _logger.Log(LogLevel.Information, $"GetAllBooks was invoked in '{stopwatch.ElapsedMilliseconds}' ms.");

            return bookDto;
        }

        public async Task AddAsync(Guid bookId, string title, string originalTitle,
            string description, string isbn, string cover, int pages, string publisher,
            DateTime publishedAt, IEnumerable<Guid> authorsId, Guid userId)
        {
            var book = await _bookRepository.GetByIsbnAsync(isbn);
            if (book != null)
            {
                throw new ServiceException(ErrorCodes.BookAlreadyExist, $"Book with ISBN '{isbn}' already exist");
            }

            try
            {
                book = new Book(bookId, title, originalTitle, description, isbn, cover.SetUpDefaultCoverWhenEmpty(), pages, publisher, publishedAt, userId);
            }
            catch (DomainException ex)
            {
                throw new ServiceException(ex, ErrorCodes.InvalidInput, ex.Message);
            }

            foreach (var id in authorsId)
            {
                var author = await _authorRepository.GetByIdAsync(id);
                if (author == null)
                {
                    throw new ServiceException(ErrorCodes.AuthorNotFound, $"Author with id '{id}' not exist");
                }

                var mappedAuthor = _mapper.Map<AuthorShortcut>(author);

                book.AddAuthor(mappedAuthor);
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

            //TODO Enhancement error msg
            var bookValidationResult = this._bookValidator.Validate(bookToUpdate);
            if (bookValidationResult.IsValid == false)
            {
                throw new ServiceException(ErrorCodes.InvalidInput, bookValidationResult.Errors.MergeResults());
            }
            
            await _bookRepository.UpdateAsync(bookToUpdate);
            _logger.LogInformation($"Book '{bookToUpdate.Title}' with id '{id}' was updated at {DateTime.UtcNow}'");

        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _bookRepository.GetOrFailAsync(id);

            await _bookRepository.RemoveAsync(book.BookId);
            _logger.LogInformation($"Book '{book.Title}' with id '{id}' was deleted at {DateTime.UtcNow}'");
        }
    }
}