using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelfy.Infrastructure.DTO.Review;

namespace Shelfy.Infrastructure.Services
{
    public interface IReviewService
    {
        Task<ReviewDto> GetAsync(Guid bookId, Guid reviewId);
        Task<IEnumerable<ReviewDto>> GetReviewsForBookAsync(Guid bookId);
        Task<IEnumerable<ReviewDto>> GetReviewsForUserAsync(Guid userId);
        Task AddAsync(int rating, string comment, Guid userId, Guid bookId);
        Task UpdateAsync(Guid reviewId, int rating, string comment);
        Task DeleteAsync(Guid bookId, Guid userId);
    }
}