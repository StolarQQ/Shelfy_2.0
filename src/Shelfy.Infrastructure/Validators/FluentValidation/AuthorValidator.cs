using System;
using System.Text.RegularExpressions;
using FluentValidation;
using Shelfy.Core.Domain;

namespace Shelfy.Infrastructure.Validators.FluentValidation
{
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