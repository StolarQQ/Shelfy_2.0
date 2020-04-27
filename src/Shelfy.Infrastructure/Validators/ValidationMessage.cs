namespace Shelfy.Infrastructure.Validators
{
    public class ValidationMessage
    {
        private string Message { get; set; }

        private ValidationMessage(string message)
        {
            Message = message;
        }

        public static ValidationMessage Create(string message)
            => new ValidationMessage(message);
    }
}