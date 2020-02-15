using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using Shelfy.Core.Domain;
using Shelfy.Core.Exceptions;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.Commands.Author;
using Shelfy.Infrastructure.DTO.Author;
using Shelfy.Infrastructure.Exceptions;
using Shelfy.Infrastructure.Extensions;
using Shelfy.Infrastructure.Pagination;
using ErrorCodes = Shelfy.Infrastructure.Exceptions.ErrorCodes;

namespace Shelfy.Infrastructure.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AuthorService(IAuthorRepository authorRepository, IMapper mapper, ILogger logger)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AuthorDto> GetByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetOrFailAsync(id);

            return _mapper.Map<AuthorDto>(author);
        }
        
        public async Task<IEnumerable<AuthorSearchDto>> BrowseByPhraseAsync(string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase) || phrase.Length < 3)
            {
               throw new ServiceException(ErrorCodes.InvalidInput, "Phrase must contains at least 3 characters");
            }
            var authors = await _authorRepository.BrowseByPhraseAsync(phrase);

            return _mapper.Map<IEnumerable<AuthorSearchDto>>(authors);
        }
     
        public async Task<PagedResult<AuthorDto>> BrowseAsync(int currentPage, int pageSize)
        {
            var authors = await _authorRepository.BrowseAsync(currentPage, pageSize);

            return _mapper.Map<PagedResult<AuthorDto>>(authors);
        }

        public async Task RegisterAsync(Guid authorId, string firstName, string lastName,
            string description, string imageUrl, DateTime? dateOfBirth, DateTime? dateOfDeath,
            string birthPlace, string authorWebsite, string authorSource, Guid userId)
        {

            var author = await _authorRepository.GetByIdAsync(authorId);
            if (author != null)
            {
                throw new ServiceException(ErrorCodes.AuthorAlreadyExist, $"Author with id '{authorId}' already exist.");
            }

            try
            {
                var authorImage = imageUrl.DefaultAuthorImageNotEmpty();

                author = new Author(authorId, firstName, lastName, description, authorImage,
                    dateOfBirth, dateOfDeath, birthPlace, authorWebsite, authorSource, userId);
            }

            catch (DomainException ex)
            {
                throw new ServiceException(ex, ErrorCodes.InvalidInput, ex.Message);
            }

            await _authorRepository.AddAsync(author);
        }

        public async Task UpdateAsync(Guid id, JsonPatchDocument<UpdateAuthor> updateAuthor)
        {
            var authorToUpdate = await _authorRepository.GetOrFailAsync(id);

            var author = _mapper.Map<UpdateAuthor>(authorToUpdate);

            updateAuthor.ApplyTo(author);

            authorToUpdate = _mapper.Map(author, authorToUpdate);
            authorToUpdate.SetFullName(authorToUpdate.FirstName, authorToUpdate.LastName);

            try
            {
                authorToUpdate.IsValid();
            }

            catch (DomainException ex)
            {
                throw new ServiceException(ex, ErrorCodes.InvalidInput, ex.Message);
            }
            
            await _authorRepository.UpdateAsync(authorToUpdate);
        }

        public async Task DeleteAsync(Guid id)
        {
            var author = await _authorRepository.GetOrFailAsync(id);

            await _authorRepository.RemoveAsync(author.AuthorId);
        }
    }
}