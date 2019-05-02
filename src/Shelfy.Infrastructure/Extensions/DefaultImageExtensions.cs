namespace Shelfy.Infrastructure.Extensions
{
    public static class DefaultImageExtensions
    {
        /// <summary>
        /// If book cover URL is null or whitespace, we set up cover to default one
        /// </summary>
        /// <param name="cover"></param>
        /// <returns></returns>
        public static string DefaultBookCoverValidation(this string cover)
        {
            var defaultCover = "https://www.stolarstate.pl/avatar/book/default.png";

            return cover.IsEmpty() ? defaultCover : cover;
        }
        /// <summary>
        /// If author Image URL is null or whitespace, we set up cover to default one
        /// </summary>
        /// <param name="authorImage"></param>
        /// <returns></returns>
        public static string DefaultAuthorImageValidation(this string authorImage)
        {
            var defaultAuthorImage = "https://www.stolarstate.pl/avatar/author/default.png";

            return authorImage.IsEmpty() ? defaultAuthorImage : authorImage;
        }
    }
}