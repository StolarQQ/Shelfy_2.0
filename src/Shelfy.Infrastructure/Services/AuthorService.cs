using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.DTO.Author;

namespace Shelfy.Infrastructure.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<AuthorDto> GetByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                throw new Exception("Author not exist");
            }

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<IEnumerable<AuthorSearchDto>> BrowseByPhraseAsync(string phrase)
        {
            if (phrase.Length < 3)
            {
                throw new Exception("Phrase must contains at least 3 characters");
            }

            var authors = await _authorRepository.BrowseByPhraseAsync(phrase);

            return _mapper.Map<IEnumerable<AuthorSearchDto>>(authors);
        }

        public async Task RegisterAsync(Guid authorId, string firstName, string lastName, string description, string imageUrl, DateTime? dateOfBirth,
            DateTime? dateOfDeath, string birthPlace, string authorWebsite, string authorSource)
        {
            var author = new Author(authorId, firstName, lastName, description, imageUrl,
                dateOfBirth, dateOfDeath, birthPlace, authorWebsite, authorSource);

            await _authorRepository.AddAsync(author);
        }
    }
}