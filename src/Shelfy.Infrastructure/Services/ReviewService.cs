using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.Commands;
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
        
        public async Task<IEnumerable<ReviewDto>> GetReviewsForUserAsync(Guid userId)
        {
            var booksReviews = await _bookRepository.GetBooksReviews();
            var reviews = new List<Review>();

            foreach (var review in booksReviews)
                if (review.UserId == userId)
                    reviews.Add(review);

            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task AddAsync(int rating, string comment, Guid userId, Guid bookId)
        {
            var book = await _bookRepository.GetOrFailAsync(bookId);
            var review = Review.Create(Guid.NewGuid(), rating, comment, userId, bookId);
            book.AddReview(review);

            await _bookRepository.UpdateAsync(book);

            _logger.LogInformation($"Review with id '{review.ReviewId}'" +
                                   $" was created for book '{book.Title}' by user with id '{userId}'");
        }

        public async Task UpdateAsync(Guid bookId, Guid userId, Guid reviewId, JsonPatchDocument<UpdateReview> updateReview)
        {
            var book = await _bookRepository.GetOrFailAsync(bookId);
            var reviewToUpdate = book.Reviews.SingleOrDefault(x => x.ReviewId == reviewId);
            if (reviewToUpdate == null)
            {
                throw new ServiceException(ErrorCodes.ReviewNotFound,
                    $"Review with id '{reviewId}' was not found for book '{book.Title}'.");
            }

            if (reviewToUpdate.UserId != userId)
            {
                throw new ServiceException(ErrorCodes.UserNotFound,
                    $"User with id '{userId}' don't have permission to update review with id '{reviewId}'.");
            }

            var review = _mapper.Map<UpdateReview>(reviewToUpdate);
            updateReview.ApplyTo(review);

            reviewToUpdate = _mapper.Map(review, reviewToUpdate);
            reviewToUpdate.IsValid();

            if (reviewToUpdate.IsValid())
            {
                await _bookRepository.UpdateAsync(book);
            }
        }

        public async Task DeleteAsync(Guid bookId, Guid userId)
        {
            var book = await _bookRepository.GetOrFailAsync(bookId);
            book.DeleteReview(userId);

            await _bookRepository.UpdateAsync(book);

            _logger.LogInformation($"Review by user {userId} was deleted for book {book.Title}");
        }
    }
}
