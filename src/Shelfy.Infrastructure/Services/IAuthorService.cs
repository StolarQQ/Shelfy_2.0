using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Shelfy.Infrastructure.Commands.Author;
using Shelfy.Infrastructure.DTO.Author;
using Shelfy.Infrastructure.Extensions;
using Shelfy.Infrastructure.Pagination;

namespace Shelfy.Infrastructure.Services
{
    public interface IAuthorService : IService
    {
        Task<AuthorDto> GetByIdAsync(Guid id);
        Task<IEnumerable<AuthorSearchDto>> BrowseByPhraseAsync(string phrase);
        Task<PagedResult<AuthorDto>> BrowseAsync(int currentPage, int pageSize); 
        Task RegisterAsync(Guid authorId, string firstName, string lastName, string description,
            string imageUrl, DateTime? dateOfBirth, DateTime? dateOfDeath,
            string birthPlace, string authorWebsite, string authorSource, Guid userId);
        Task UpdateAsync(Guid id, JsonPatchDocument<UpdateAuthor> updateAuthor);
        Task DeleteAsync(Guid id);
    }
}