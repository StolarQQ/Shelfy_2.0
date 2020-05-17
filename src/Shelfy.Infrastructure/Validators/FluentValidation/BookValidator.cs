using FluentValidation;
using Shelfy.Core.Domain;

namespace Shelfy.Infrastructure.Validators.FluentValidation
{
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
}