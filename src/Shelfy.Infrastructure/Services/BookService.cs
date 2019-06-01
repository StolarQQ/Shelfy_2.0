using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Shelfy.Core.Domain;
using Shelfy.Core.Exceptions;
using Shelfy.Core.Helper;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.Commands.Book;
using Shelfy.Infrastructure.DTO.Author;
using Shelfy.Infrastructure.DTO.Book;
using Shelfy.Infrastructure.Exceptions;
using Shelfy.Infrastructure.Extensions;
using ErrorCodes = Shelfy.Infrastructure.Exceptions.ErrorCodes;

namespace Shelfy.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository,
            IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BookDetailsDto> GetByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetOrFailAsync(id);

            var authors = await GetAuthors(book.AuthorsIds);
            var bookDto = SetUpBook(book, authors);

            return bookDto;
        }

        public async Task<BookDetailsDto> GetByIsbnAsync(string isbn)
        {
            var book = await _bookRepository.GetOrFailAsync(isbn);

            var authors = await GetAuthors(book.AuthorsIds);
            var bookDto = SetUpBook(book, authors);

            return bookDto;
        }

        public async Task<PagedResult<BookDto>> BrowseAsync(int currentPage, int pageSize, string query)
        {
            _logger.LogInformation("Fetching Data from Book repository");
            var books = await _bookRepository.BrowseAsync(currentPage, pageSize, query);

            var allBooks = new List<BookDto>();

            foreach (var book in books.Source)
            {
                var authors = await GetAuthors(book.AuthorsIds);
                allBooks.Add(SetupBookForBrowse(book, authors));
            }

            var mappedResult = _mapper.Map<PagedResult<BookDto>>(books);
            mappedResult.Source = allBooks;

            return mappedResult;
        }

        public async Task AddAsync(Guid bookId, string title, string originalTitle,
            string description, string isbn, string cover, int pages, string publisher,
            DateTime publishedAt, IEnumerable<Guid> authorsId, Guid userId)
        {
            var book = await _bookRepository.GetByIsbnAsync(isbn);
            if (book != null)
            {
                throw new ServiceException(ErrorCodes.BookAlreadyExist, $"Book with ISBN {isbn} already exist");
            }

            try
            {
                var validCover = cover.DefaultBookCoverNotEmpty();
                book = new Book(bookId, title, originalTitle, description, isbn, validCover, pages, publisher, publishedAt, userId);
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
                    throw new ServiceException(ErrorCodes.AuthorNotFound, $"Author with id '{id} not exist");
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

            try
            {
                bookToUpdate.IsValid();
            }
            catch (DomainException ex)
            {
                throw new ServiceException(ex, ErrorCodes.InvalidInput, ex.Message);
            }

            await _bookRepository.UpdateAsync(bookToUpdate);
            _logger.LogInformation($"Book '{bookToUpdate.Title}' with id '{id} was updated at {DateTime.UtcNow}'");

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
        private BookDetailsDto SetUpBook(Book book, IEnumerable<Author> authors)
        {
            var bookDto = _mapper.Map<BookDetailsDto>(book);
            var authorsDto = _mapper.Map<IEnumerable<AuthorFullNameDto>>(authors);
            bookDto.Authors = authorsDto;

            return bookDto;
        }

        /// <summary>
        /// Set up books for browsing
        /// </summary>
        /// <param name="book"></param>
        /// <param name="authors"></param>
        /// <returns></returns>
        private BookDto SetupBookForBrowse(Book book, IEnumerable<Author> authors)
        {
            var bookDto = _mapper.Map<BookDto>(book);
            var authorsDto = _mapper.Map<IEnumerable<AuthorNameDto>>(authors);
            bookDto.Authors = authorsDto;

            return bookDto;
        }
    }
}