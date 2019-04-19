using System;
using System.Threading.Tasks;
using Shelfy.Infrastructure.DTO.Author;

namespace Shelfy.Infrastructure.Services
{
    public interface IAuthorService
    {
        Task<AuthorDto> GetAsync(Guid id);
        Task RegisterAsync(Guid authorId, string firstName, string lastName, string description,
            string imageUrl, DateTime? dateOfBirth, DateTime? dateOfDeath,
            string birthPlace, string authorWebsite, string authorSource);
    }
}