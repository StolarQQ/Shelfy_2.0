using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shelfy.Core.Domain;
using Shelfy.Infrastructure.Extensions;

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
            

            for (var i = 0; i < 10000; i++)
            {

                var userId = Guid.NewGuid();
                await _userService.RegisterAsync(userId,
                    $"email{i}@gmail.com", $"username{i}", "secret123");

                var authorId = Guid.NewGuid();
                await _authorService.RegisterAsync(authorId, DataGenerator.GenerateFirstName(), DataGenerator.GenerateFirstName()
                    , DataGenerator.GenerateDescription(), null, DataGenerator.GenerateDate(), null, DataGenerator.GenerateCity(), DataGenerator.GenerateWebsite(),
                    DataGenerator.GenerateWebsite(), userId);

                var bookId = Guid.NewGuid();
                var authorsId = new List<Guid> { authorId };

                await _bookService.AddAsync(bookId, DataGenerator.GenerateTitles(), null, DataGenerator.GenerateDescription(),
                    DataGenerator.GenerateRandomIsbn(13), null, 999, DataGenerator.GenerateCity(), DateTime.Now, authorsId, userId);

                await _reviewService.AddAsync(random.Next(1, 7), DataGenerator.GenerateDescription(), userId, bookId);
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
    }
}