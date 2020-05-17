using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Shelfy.Core.Domain;
using Shelfy.Core.Exceptions;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.Commands.Review;
using Shelfy.Infrastructure.DTO.Review;
using Shelfy.Infrastructure.Exceptions;
using Shelfy.Infrastructure.Extensions;
using ErrorCodes = Shelfy.Infrastructure.Exceptions.ErrorCodes;

namespace Shelfy.Infrastructure.Services
{
    
    public class ReviewService : IReviewService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ReviewService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<Review> _reviewValidator;

        public ReviewService(IBookRepository bookRepository, ILogger<ReviewService> logger, IMapper mapper, IValidator<Review> reviewValidator, IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _mapper = mapper;
            _reviewValidator = reviewValidator;
            _userRepository = userRepository;
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

        public async Task<IEnumerable<UserReviewDto>> GetReviewsForUserAsync(Guid userId)
        {
            var user = await _userRepository.GetOrFailAsync(userId);

            return _mapper.Map<IEnumerable<UserReviewDto>>(user.Reviews);
        }

        public async Task AddAsync(int rating, string comment, Guid userId, Guid bookId)
        {
            var book = await _bookRepository.GetOrFailAsync(bookId);
            var user = await _userRepository.GetOrFailAsync(userId);

            try
            {
                var review = Review.Create(Guid.NewGuid(), rating, comment, userId, bookId);
                book.AddReview(review);
                user.AddReview(_mapper.Map<ReviewShortcut>(review));
            }
            catch (DomainException ex)
            {
                throw new ServiceException(ex, ErrorCodes.InvalidInput, ex.Message);
            }
            
            await _bookRepository.UpdateAsync(book);
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"Review was created for book '{book.Title} by user with id '{userId}'");

        }

        public async Task UpdateAsync(Guid bookId, Guid userId, JsonPatchDocument<UpdateReview> updateReview)
        {
            var book = await _bookRepository.GetOrFailAsync(bookId);
            var reviewToUpdate = book.Reviews.SingleOrDefault(x => x.CreatorId == userId);
            if (reviewToUpdate == null)
            {
                throw new ServiceException(ErrorCodes.ReviewNotFound,
                    $"User with id '{userId}' was not created review for book '{book.Title}'.");
            }

            var review = _mapper.Map<UpdateReview>(reviewToUpdate);
            updateReview.ApplyTo(review);

            reviewToUpdate = _mapper.Map(review, reviewToUpdate);

            var reviewValidationResult = this._reviewValidator.Validate(reviewToUpdate);
            if (reviewValidationResult.IsValid == false)
            {
                throw new ServiceException(ErrorCodes.InvalidInput, reviewValidationResult.Errors.MergeResults());
            }
        
            await _bookRepository.UpdateAsync(book);
        }

        public async Task DeleteAsync(Guid bookId, Guid userId)
        {
            var book = await _bookRepository.GetOrFailAsync(bookId);

            try
            {
                book.DeleteReview(userId);
            }
            catch (DomainException ex)
            {
                throw new ServiceException(ex, ErrorCodes.InvalidInput, ex.Message);
            }

            await _bookRepository.UpdateAsync(book);

            _logger.LogInformation($"Review by user {userId} was deleted for book {book.Title}");
        }
    }
}
