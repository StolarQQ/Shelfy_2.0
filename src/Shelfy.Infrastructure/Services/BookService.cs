using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
     
        public async Task<BookDto> GetAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                throw new ArgumentException($"Book with id '{id}' was not found.");
            }
            
            var authors = await GetAuthors(book.Authors);
            var bookDto = SetUpBook(book, authors);

            return bookDto;
        }

        public async Task<BookDto> GetAsync(string isbn)
        {
            var book = await _bookRepository.GetByIsbnAsync(isbn);
            if (book == null)
            {
                throw new ArgumentException($"Book with ISBN '{isbn}' was not found.");
            }

            var authors = await GetAuthors(book.Authors);
            var bookDto = SetUpBook(book, authors);

            return bookDto;
        }

        // TODO This method will be replace by pagination one
        public async Task<IEnumerable<BookDto>> BrowseAsync()
        {
            var books = await _bookRepository.BrowseAsync();

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task AddAsync(string title, string originalTitle,
            string description, string isbn, int pages, string publisher,
            DateTime publishedAt, IEnumerable<Guid> authorsId)
        {
            var book = await _bookRepository.GetByIsbnAsync(isbn);
            if (book != null)
            {
                throw new Exception($"Book with ISBN {isbn} already exist");
            }

            book = new Book(title, originalTitle, description, isbn, pages, publisher, publishedAt);

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
        }

        public async Task UpdateAsync(Guid id, JsonPatchDocument<UpdateBook> patchBook)
        {
            var existedBook = await _bookRepository.GetByIdAsync(id);
            if (existedBook == null)
            {
                throw new ArgumentException($"Book with id '{id}' was not found");
            }

            var book = _mapper.Map<UpdateBook>(existedBook);
            patchBook.ApplyTo(book);
            
            // Mapping changes to bookToUpdate
            existedBook = _mapper.Map(book, existedBook);
            
            if (existedBook.IsValid())
            {
                await _bookRepository.UpdateAsync(existedBook);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                throw new ArgumentException($"Book with id '{id}' was not found");
            }

            await _bookRepository.RemoveAsync(id);
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