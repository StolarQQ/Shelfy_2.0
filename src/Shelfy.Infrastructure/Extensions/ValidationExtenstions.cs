using System;
using Shelfy.Core.Domain;

namespace Shelfy.Infrastructure.Extensions
{
    public static class ValidationExtensions
    {
        /// <summary>
        /// Validate book in UpdateAsync, after mapping in BookService
        /// </summary>
        public static bool IsValid(this Book book)
        {
            book.SetTitle(book.Title);
            book.SetDescription(book.Title);
            book.SetIsbn(book.ISBN);
            book.SetPages(book.Pages);
            book.SetPublisher(book.Publisher);

            return true;
        }

        /// <summary>
        /// Validate author before update
        /// </summary>
        public static bool IsValid(this Author author)
        {
            author.SetFirstName(author.FirstName);
            author.SetLastName(author.FirstName);
            author.SetDescription(author.Description);
            author.SetDateOfBirth(author.DateOfBirth);
            author.SetDateOfDeath(author.DateOfDeath);
            author.SetBirthPlace(author.BirthPlace);
            author.SetAuthorWebsite(author.AuthorWebsite);
            author.SetAuthorSource(author.AuthorSource);

            return true;
        }

        /// <summary>
        /// Validate password before generate hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="min">Minimum length of password</param>
        /// <param name="max">Maximum length of password</param>
        public static void PasswordValidation(this string password, int min = 5, int max = 25)
        {
            if (password.IsEmpty())
            {
                throw new Exception("Password cannot be empty.");
            }

            if (password.Length < min)
            {
                throw new Exception($"Password must contain at least {min} characters.");
            }

            if (password.Length > max)
            {
                throw new Exception($"Password cannot contain more than {max} characters.");
            }
        }
    }
}
