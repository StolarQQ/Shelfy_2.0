using System;
using System.Text.RegularExpressions;
using FluentValidation;
using Shelfy.Core.Domain;

namespace Shelfy.Infrastructure.Validators.FluentValidation
{
    public class ReviewValidator : AbstractValidator<Review>
    {
        public ReviewValidator()
        {
            RuleFor(x => x.Rating).InclusiveBetween(1, 6).WithMessage("Rating must inclusive between 1 and 6");
            RuleFor(x => x.Comment).NotEmpty().Length(5, 300);
        }
    }
    
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty().Length(15, 500);
            RuleFor(x => x.ISBN).Length(13);
            RuleFor(x => x.Pages).LessThan(1);
            RuleFor(x => x.Publisher).NotEmpty();
        }
    }  
    
    public class AuthorValidator : AbstractValidator<Author>
    {
        private static readonly Regex ImageUrlRegex = new Regex("(http(s?):)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)");
        private static readonly Regex UrlRegex = new Regex(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$");

        public AuthorValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().Length(2, 20);
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Description).Empty().Length(15, 500);
            RuleFor(x => x.DateOfBirth).GreaterThan(DateTime.Today);
            RuleFor(x => x).Must(ValidDateOfDeath);
            RuleFor(x => x.AuthorWebsite).Matches(UrlRegex);
            RuleFor(x => x.AuthorSource).Matches(UrlRegex);
            RuleFor(x => x.ImageUrl).Matches(ImageUrlRegex);
        }

        private bool ValidDateOfDeath(Author author)
            => author.DateOfBirth != author.DateOfDeath;
    }
}
