using System;
using Shelfy.Core.Domain;
using Shelfy.Infrastructure.Exceptions;

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
            book.SetDescription(book.Description);
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
            author.SetLastName(author.LastName);
            author.SetDescription(author.Description);
            author.SetDateOfBirth(author.DateOfBirth);
            author.SetDateOfDeath(author.DateOfDeath);
            author.SetBirthPlace(author.BirthPlace);
            author.SetAuthorWebsite(author.AuthorWebsite);
            author.SetAuthorSource(author.AuthorSource);

            return true;
        }

        /// <summary>
        /// Validate review before update
        /// </summary>
        public static bool IsValid(this Review review)
        {
            review.SetRating(review.Rating);
            review.SetComment(review.Comment);

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
                throw new ServiceException(ErrorCodes.InvalidPassword, "Password cannot be empty.");
            }

            if (password.Length < min)
            {
                throw new ServiceException(ErrorCodes.InvalidPassword, $"Password must contain at least {min} characters.");
            }

            if (password.Length > max)
            {
                throw new ServiceException(ErrorCodes.InvalidPassword, $"Password cannot contain more than {max} characters.");
            }
        }
    }
}
