using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson.IO;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.DTO.Book;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Shelfy.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BookDto> GetAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                throw new ArgumentException($"Book with id '{id}' was not found.");
            }

            return _mapper.Map<BookDto>(book);

        }

        public async Task<BookDto> GetAsync(string isbn)
        {
            var book = await _bookRepository.GetByIsbnAsync(isbn);
            if (book == null)
            {
                throw new ArgumentException($"Book with ISBN '{isbn}' was not found.");
            }

            return _mapper.Map<BookDto>(book);
        }

        public async Task<IEnumerable<BookDto>> BrowseAsync()
        {
            var books = await _bookRepository.BrowseAsync();

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task AddAsync(string title, string originalTitle,
            string description, string isbn,
            int pages, string publisher, DateTime publishedAt)
        {
            var book = await _bookRepository.GetByIsbnAsync(isbn);
            if (book != null)
            {
                throw new Exception($"Book with ISBN {isbn} already exist");
            }

            book = new Book(title, originalTitle, description, isbn, pages, publisher, publishedAt);
            
            await _bookRepository.AddAsync(book);
        }

        public async Task UpdateAsync(Book book)
        {
            //var existedBook = await _bookRepository.GetByIdAsync(book.BookId);
            //if (existedBook == null)
            //{
            //    throw new ArgumentException($"Book with id '{book.BookId}' was not found");
            //}

            //await _bookRepository.UpdateAsync(book);
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
    }
}