using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelfy.Infrastructure.DTO.Author;

namespace Shelfy.Infrastructure.Services
{
    public interface IAuthorService
    {
        Task<AuthorDto> GetByIdAsync(Guid id);
        Task<IEnumerable<AuthorSearchDto>> BrowseByPhraseAsync(string phrase);
        Task RegisterAsync(Guid authorId, string firstName, string lastName, string description,
            string imageUrl, DateTime? dateOfBirth, DateTime? dateOfDeath,
            string birthPlace, string authorWebsite, string authorSource);
    }
}