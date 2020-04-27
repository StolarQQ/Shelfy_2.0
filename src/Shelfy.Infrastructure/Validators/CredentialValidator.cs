using System.Collections.Generic;
using System.Linq;
using Shelfy.Infrastructure.Extensions;

namespace Shelfy.Infrastructure.Validators
{
    public class CredentialValidator : ICredentialValidator
    {
        public ValidationResult ValidatePassword(string password)
        {
            var validationResults = new List<ValidationMessage>();

            if (password.IsEmpty())
                validationResults.Add(ValidationMessage.Create("Password cannot be empty."));

            if (password.Length < 5)
                validationResults.Add(ValidationMessage.Create("Password must contain at least 5 characters."));

            if (password.Length > 20)
                validationResults.Add(ValidationMessage.Create("Password cannot contain more than 30 characters."));

            if(this.ContainsDigit(password) == false)
                validationResults.Add(ValidationMessage.Create("Password should contains at least one digit"));

            if(this.ContainsSpecialCharacters(password) == false)
                validationResults.Add(ValidationMessage.Create("Password should contains at least one special character"));

            return new ValidationResult(validationResults);
        }

        public bool ContainsSpecialCharacters(string credential)
            => credential.Any(char.IsSymbol); 
        public bool ContainsDigit(string credential)
            => credential.Any(char.IsDigit);
    }
}