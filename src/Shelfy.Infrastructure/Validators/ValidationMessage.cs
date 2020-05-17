namespace Shelfy.Infrastructure.Validators
{
    public class ValidationMessage
    {
        public string Message { get; }

        private ValidationMessage(string message)
        {
            Message = message;
        }

        public static ValidationMessage Create(string message)
            => new ValidationMessage(message);
    }
}