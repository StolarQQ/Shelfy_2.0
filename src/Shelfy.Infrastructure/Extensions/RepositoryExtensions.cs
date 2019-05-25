using System;
using System.Threading.Tasks;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.Exceptions;

namespace Shelfy.Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<Book> GetOrFailAsync(this IBookRepository bookRepository, Guid id)
        {
            var book = await bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                throw new ServiceException(ErrorCodes.BookNotFound, $"Book with id '{id}' was not found.");
            }

            return book;
        }

        public static async Task<Book> GetOrFailAsync(this IBookRepository bookRepository, string isbn)
        {
            var book = await bookRepository.GetByIsbnAsync(isbn);

            if (book == null)
            {
                throw new ServiceException(ErrorCodes.BookNotFound, $"Book with isbn '{isbn}' was not found.");
            }

            return book;
        }

        public static async Task<Author> GetOrFailAsync(this IAuthorRepository authorRepository, Guid id)
        {
            var author = await authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                throw new ServiceException(ErrorCodes.AuthorNotFound, $"Author with id '{id}' was not found.");
            }

            return author;
        }

        public static async Task<User> GetOrFailAsync(this IUserRepository userRepository, Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound, $"User with id '{id}' was not found.");
            }

            return user;
        }

        public static async Task<User> GetOrFailAsync(this IUserRepository userRepository, string username)
        {
            var user = await userRepository.GetByUsernameAsync(username);

            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound, $"User with username '{username}' was not found.");
            }

            return user;
        }
    }
}
