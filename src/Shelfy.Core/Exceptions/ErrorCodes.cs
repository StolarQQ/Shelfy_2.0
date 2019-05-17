namespace Shelfy.Core.Exceptions
{
    public static class ErrorCodes
    {
        public static string InvalidFirstName => "invalid_firstname";
        public static string InvalidLastName => "invalid_firstname";
        public static string InvalidDescription => "invalid_description";
        public static string InvalidDateOfBirth => "invalid_dateOfBirth";
        public static string InvalidDateOfDeath => "invalid_dateOfDeath";
        public static string InvalidBirthPlace => "invalid_birthPlace";
        public static string InvalidAuthorSource => "invalid_authorSource";
        public static string InvalidAuthorWebsite => "invalid_authorWebsite";
        public static string InvalidImageUrl => "invalid_image_url";
        public static string InvalidTitle => "invalid_title";
        public static string InvalidIsbn => "invalid_isbn";
        public static string InvalidCover => "invalid_cover";
        public static string InvalidPublisher => "invalid_publisher";
        public static string InvalidEmail => "invalid_email";
        public static string InvalidPassword => "invalid_password";
        public static string InvalidRole => "invalid_role";
        public static string InvalidState => "invalid_state";
        public static string InvalidUsername => "invalid_username";
        public static string InvalidAvatar => "invalid_avatar";
        public static string InvalidPages => "invalid_pages";
        public static string InvalidRating => "invalid_rating";
        public static string InvalidComment => "invalid_comment";
        public static string BookNotFound => "book_not_found";
        public static string AuthorNotFound => "author_not_found";
        public static string ReviewNotFound => "review_not_found";
        public static string EmailNotFound => "email_not_found";
        public static string UserNotFound => "user_not_found";
        public static string EmailInUse => "email_in_use";
        public static string UserNameInUse => "username_in_use";
        public static string UserAlreadyAdded => "user_already_added";
        public static string ReviewAlreadyAdded => "review_already_added";
        public static string BookAlreadyAdded => "book_already_added";
        public static string AuthorAlreadyAdded => "author_already_added";
        public static string AccountAlreadyActivated => "account_already_activated";
        public static string AccountAlreadyUnlocked => "account_already_locked";
        public static string AccountAlreadyLocked => "account_already_locked";
    }
}
