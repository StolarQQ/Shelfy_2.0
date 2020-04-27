using System.Collections.Generic;

namespace Shelfy.Infrastructure.Validators
{
    public class ValidationResult
    {
        public IList<ValidationMessage> ValidationMessage { get; private set; }
        public bool IsValid => ValidationMessage.Count == 0;

        public ValidationResult(IList<ValidationMessage> validationMessage)
        {
            ValidationMessage = validationMessage;
        }
    }
}