using System;
using System.Text;
using Bogus;

namespace Shelfy.Infrastructure.Extensions
{
    public static class DataGenerator
    {
        public static string GenerateFirstName()
            => new Faker().Name.FirstName();

        public static string GenerateDescription()
            => new Faker().Lorem.Paragraph();

        public static string GenerateCity()
            => new Faker().Address.City();

        public static DateTime GenerateDate()
            => new Faker().Date.Past(50, DateTime.Now);

        public static string GenerateWebsite()
            => new Faker().Internet.Url();

        public static string GenerateTitles()
            => new Faker().Commerce.Product();

        public static string GenerateRandomIsbn(int stringLength)
        {
            var rnd = new Random();
            var sb = new StringBuilder();
            var randomString = "1234567890";

            for (var i = 0; i < stringLength; i++)
                sb.Append(randomString[rnd.Next(0, randomString.Length)]);

            return sb.ToString();
        }
    }
}