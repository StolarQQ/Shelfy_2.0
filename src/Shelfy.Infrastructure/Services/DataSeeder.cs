using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shelfy.Core.Domain;

namespace Shelfy.Infrastructure.Services
{
    public class DataSeeder : IDataSeeder
    {
        private readonly IUserService _userService;
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;
        private readonly IReviewService _reviewService;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(IUserService userService, IAuthorService authorService,
            IBookService bookService, IReviewService reviewService, ILogger<DataSeeder> logger, IConfiguration config)
        {
            _userService = userService;
            _authorService = authorService;
            _bookService = bookService;
            _reviewService = reviewService;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            var users = await _userService.BrowseAsync();

            if (users.Source.Any())
            {
                return;
            }

            var random = new Random();

            _logger.LogInformation("Seeding testing data !");

            for (int i = 0; i < 150; i++)
            {
                var userId = Guid.NewGuid();
                await _userService.RegisterAsync(userId,
                    $"email{i}@gmail.com", $"username{i}", "secret123");

                var authorId = Guid.NewGuid();
                await _authorService.RegisterAsync(authorId, GenerateFirstName(), GenerateFirstName()
                    , GenerateDescription(), null, GenerateDate(), null, GenerateCity(), GenerateWebsite(),
                    GenerateWebsite(), userId);

                var bookId = Guid.NewGuid();
                var authorsId = new List<Guid> { authorId };
                await _bookService.AddAsync(bookId, GenerateTitles(), null, GenerateDescription(),
                    GenerateRandomIsbn(13), null, 999, GenerateCity(), DateTime.Now, authorsId, userId);

                await _reviewService.AddAsync(random.Next(1, 6), GenerateDescription(), userId, bookId);
            }

            for (int i = 0; i < 3; i++)
            {
                var adminId = Guid.NewGuid();
                await _userService.RegisterAsync(adminId,
                    $"admin{i}@gmail.com", $"admin{i}", "admin123", Role.Admin);

                var moderatorId = Guid.NewGuid();
                await _userService.RegisterAsync(moderatorId,
                    $"moderator{i}@gmail.com", $"moderator{i}", "secret123", Role.Moderator);
            }
        }

        private string GenerateFirstName()
        {
            var faker = new Faker();
            var firstName = faker.Name.FirstName();

            return firstName;
        }

        private string GenerateDescription()
        {
            var faker = new Faker();
            var desc = faker.Lorem.Paragraph();

            return desc;
        }

        private string GenerateCity()
        {
            var faker = new Faker();
            var city = faker.Address.City();

            return city;
        }

        private DateTime GenerateDate()
        {
            var faker = new Faker();
            var date = faker.Date.Past(50, DateTime.Now);

            return date;
        }

        private string GenerateWebsite()
        {
            var faker = new Faker();
            var url = faker.Internet.Url();

            return url;
        }

        private string GenerateTitles()
        {
            var faker = new Faker();
            var fakeTitle = faker.Commerce.Product();

            return fakeTitle;
        }

        private string GenerateRandomIsbn(int stringLength)
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