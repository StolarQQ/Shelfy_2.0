using System;
using System.Text;

namespace Shelfy.Tests
{
    public static class Helper
    {
        /// <summary>
        /// Helper method for generating random string with fixed length
        /// </summary>
        /// <param name="stringLength"></param>
        /// <returns></returns>
        public static string GenerateRandomString(int stringLength)
        {
            var rnd = new Random();
            var sb = new StringBuilder();
            var randomString = "ACDASDXASLDJASDJASDLJSADBVOHGDASDASDASHDASIHDO";

            for (var i = 0; i < stringLength; i++)
                sb.Append(randomString[rnd.Next(0, randomString.Length)]);

            return sb.ToString();
        }
    }
}