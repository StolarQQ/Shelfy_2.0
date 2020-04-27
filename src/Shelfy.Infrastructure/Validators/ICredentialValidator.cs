namespace Shelfy.Infrastructure.Validators
{
    public interface ICredentialValidator
    {
        ValidationResult ValidatePassword(string password);
        bool ContainsSpecialCharacters(string credential);
        bool ContainsDigit(string credential);
    }
}