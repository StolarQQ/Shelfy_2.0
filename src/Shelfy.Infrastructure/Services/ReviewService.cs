using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.DTO.Review;
using Shelfy.Infrastructure.Exceptions;
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
        
        public async Task<ReviewDto> GetAsync(Guid bookId, Guid reviewId)
        {
            var book = await _bookRepository.GetOrFailAsync(bookId);

            var review = book.Reviews.FirstOrDefault(x => x.ReviewId == reviewId);
            if (review == null)
            {
                throw new ServiceException(ErrorCodes.ReviewNotFound, $"Review with id {reviewId} not exist for book {book.Title}");
            }
                    
            return _mapper.Map<ReviewDto>(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsForBookAsync(Guid bookId)
        {
            var book = await _bookRepository.GetOrFailAsync(bookId);

            return _mapper.Map<IEnumerable<ReviewDto>>(book.Reviews);
        }

        // TODO DUPLICATE DATA IN USER !! 
        public async Task<IEnumerable<ReviewDto>> GetReviewsForUserAsync(Guid userId)
        {
            var books = await _bookRepository.GetAll();
            var reviews = new List<Review>();

            foreach (var book in books)
            {
                var allReviews = book.Reviews;

                foreach (var review in allReviews)
                    if (review.UserId == userId)
                        reviews.Add(review);
            }

            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task AddAsync(int rating, string comment, Guid userId, Guid bookId)
        {
            var book = await _bookRepository.GetOrFailAsync(bookId);
            var review = Review.Create(Guid.NewGuid(), rating, comment, userId, bookId);
            book.AddReview(review);

            _logger.LogInformation($"Review with id '{review.ReviewId}'" +
                                   $" was created for book '{book.Title}' by user with id '{userId}'");
        }

        public async Task UpdateAsync(Guid reviewId, int rating, string comment)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid bookId, Guid userId)
        {
            var book = await _bookRepository.GetOrFailAsync(bookId);
            book.DeleteReview(userId);

            _logger.LogInformation($"Review by user {userId} was deleted for book {book.Title}");
        }
    }
}
