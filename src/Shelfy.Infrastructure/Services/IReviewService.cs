using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.DTO.Review;

namespace Shelfy.Infrastructure.Services
{
    public interface IReviewService
    {
        Task<ReviewDto> GetAsync(Guid bookId, Guid reviewId);
        Task<IEnumerable<ReviewDto>> GetReviewsForBookAsync(Guid bookId);
        Task<IEnumerable<ReviewDto>> GetReviewsForUserAsync(Guid userId);
        Task AddAsync(int rating, string comment, Guid userId, Guid bookId);
        Task UpdateAsync(Guid bookId, Guid userId, Guid reviewId, JsonPatchDocument<UpdateReview> updateReview);
        Task DeleteAsync(Guid bookId, Guid userId);
    }
}