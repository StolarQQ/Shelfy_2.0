namespace Shelfy.Infrastructure.Exceptions
{
    public static class ErrorCodes
    {
        public static string InvalidEmail => "invalid_email";
        public static string InvalidCredentials => "invalid_credentials";
        public static string InvalidPassword => "invalid_password";
        public static string EmailInUse => "email_in_use";
        public static string UsernameInUse => "username_in_use";
        public static string UserNotFound => "user_not_found";
        public static string AuthorNotFound => "author_not_found";
        public static string ReviewNotFound => "review_not_found";
        public static string EmailNotFound => "email_not_found";
        public static string BookNotFound => "book_not_found";
        public static string AuthorAlreadyExist => "author_already_exist";
        public static string BookAlreadyExist => "book_already_exist";
        public static string InvalidInput => "invalid_input";
    }
}