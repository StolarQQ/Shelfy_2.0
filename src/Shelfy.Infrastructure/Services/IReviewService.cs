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
        Task AddAsync(int rating, string comment,Guid userId, Guid bookId);
        Task DeleteAsync(Guid bookId, Guid userId);
    }
}