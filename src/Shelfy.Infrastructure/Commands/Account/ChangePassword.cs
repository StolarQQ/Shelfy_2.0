namespace Shelfy.Infrastructure.Commands.Account
{
    public class ChangePassword
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}