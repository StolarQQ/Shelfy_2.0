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
}
