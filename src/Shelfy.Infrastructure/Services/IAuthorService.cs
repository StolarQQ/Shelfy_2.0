using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.DTO.Author;

namespace Shelfy.Infrastructure.Services
{
    public interface IAuthorService
    {
        Task<AuthorDto> GetByIdAsync(Guid id);
        Task<IEnumerable<AuthorSearchDto>> BrowseByPhraseAsync(string phrase);
        Task<IEnumerable<AuthorDto>> BrowseAsync(); 
        Task RegisterAsync(Guid authorId, string firstName, string lastName, string description,
            string imageUrl, DateTime? dateOfBirth, DateTime? dateOfDeath,
            string birthPlace, string authorWebsite, string authorSource, Guid userId);
        Task UpdateAsync(Guid id, JsonPatchDocument<UpdateAuthor> updateAuthor);
        Task DeleteAsync(Guid id);
    }
}