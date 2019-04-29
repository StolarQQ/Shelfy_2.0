using System;
using System.Threading.Tasks;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.Repositories;

namespace Shelfy.Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<Book> GetOrFailAsync(this IBookRepository bookRepository, Guid id)
        {
            var book = await bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                throw new ArgumentException($"Book with id '{id}' was not found.");
            }

            return book;
        }

        public static async Task<Author> GetOrFailAsync(this IAuthorRepository authorRepository, Guid id)
        {
            var author = await authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                throw new ArgumentException($"Author with id '{id}' was not found.");
            }

            return author;
        }

        public static async Task<User> GetOrFailAsync(this IUserRepository userRepository, Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new ArgumentException($"User with id '{id}' was not found.");
            }

            return user;
        }

        public static async Task<User> GetOrFailAsync(this IUserRepository userRepository, string username)
        {
            var user = await userRepository.GetByUsernameAsync(username);

            if (user == null)
            {
                throw new ArgumentException($"User with email '{username}' was not found.");
            }

            return user;
        }
    }
}
