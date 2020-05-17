namespace Shelfy.Infrastructure.Extensions
{
    public static class DefaultImageExtensions
    {
        /// <summary>
        /// If book cover URL is null or whitespace, we set up cover to default one
        /// </summary>
        /// <param name="cover"></param>
        /// <returns></returns>
        public static string SetUpDefaultCoverWhenEmpty(this string cover)
            => cover.IsEmpty() ? "https://www.stolarstate.pl/avatar/book/default.png" : cover;
        
        /// <summary>
        /// If author Image URL is null or whitespace, we set up cover to default one
        /// </summary>
        /// <param name="authorImage"></param>
        /// <returns></returns>
        public static string DefaultAuthorImageNotEmpty(this string authorImage)
            => authorImage.IsEmpty() ? "https://www.stolarstate.pl/avatar/author/default.png" : authorImage;
        
    }
}