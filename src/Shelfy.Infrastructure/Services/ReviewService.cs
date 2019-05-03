using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.DTO.Review;
using Shelfy.Infrastructure.Extensions;

namespace Shelfy.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<ReviewService> _logger;
        private readonly IMapper _mapper;

        public ReviewService(IBookRepository bookRepository, ILogger<ReviewService> logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public Task<ReviewDto> GetAsync(Guid bookId, Guid reviewId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsForBookAsync(Guid bookId)
        {
            var book = await _bookRepository.GetOrFailAsync(bookId);

            return _mapper.Map<IEnumerable<ReviewDto>>(book.Reviews);
        }

        public async Task AddAsync(int rating, string comment, Guid userId, Guid bookId)
        {
            var book = await _bookRepository.GetOrFailAsync(bookId);
            var review = Review.Create(Guid.NewGuid(), rating, comment, userId, bookId);
            book.AddReview(review);

            _logger.LogInformation($"Review with id '{review.ReviewId}'" +
                                   $" was created for book '{book.Title}' by user with id '{userId}'");
        }

        public async Task DeleteAsync(Guid bookId, Guid userId)
        {
            var book = await _bookRepository.GetOrFailAsync(bookId);
            book.DeleteReview(userId);

            _logger.LogInformation($"Review by user {userId} was deleted for book {book.Title}");
        }
    }
}
