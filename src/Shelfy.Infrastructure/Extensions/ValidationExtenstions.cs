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
    }
}
