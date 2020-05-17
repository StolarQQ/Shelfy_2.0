using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Shelfy.Infrastructure.Validators;

namespace Shelfy.Infrastructure.Extensions
{
    public static class ValidationMessageExtension
    {
        public static string MergeResults(this IEnumerable<ValidationMessage> validationMessages)
            => string.Join(", ", validationMessages.Select(x => x.Message));

        public static string MergeResults(this IEnumerable<ValidationFailure> validationMessages)
            => string.Join(", ", validationMessages.Select(x => x.ErrorMessage));
    }
}