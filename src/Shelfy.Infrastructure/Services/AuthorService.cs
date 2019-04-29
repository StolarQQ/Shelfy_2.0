﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.DTO.Author;
using Shelfy.Infrastructure.Extensions;

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
            if (string.IsNullOrWhiteSpace(phrase) || phrase.Length < 3)
            {
                throw new Exception("Phrase must contains at least 3 characters");
            }

            var authors = await _authorRepository.BrowseByPhraseAsync(phrase);

            return _mapper.Map<IEnumerable<AuthorSearchDto>>(authors);
        }

        public async Task<IEnumerable<AuthorDto>> BrowseAsync()
        {
            var authors = await _authorRepository.BrowseAsync();

            return _mapper.Map<IEnumerable<AuthorDto>>(authors);
        }

        public async Task RegisterAsync(Guid authorId, string firstName, string lastName, string description, string imageUrl, DateTime? dateOfBirth,
            DateTime? dateOfDeath, string birthPlace, string authorWebsite, string authorSource)
        {
            var author = new Author(authorId, firstName, lastName, description, imageUrl,
                dateOfBirth, dateOfDeath, birthPlace, authorWebsite, authorSource);

            var authorExist = await _authorRepository.GetByIdAsync(author.AuthorId);
            if (authorExist != null)
            {
                throw new Exception($"Author with id '{author.AuthorId}' already exist");
            }

            await _authorRepository.AddAsync(author);
        }

        public async Task UpdateAsync(Guid id, JsonPatchDocument<UpdateAuthor> updateAuthor)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                throw new Exception($"Author with id '{id}' not exist");
            }

            var authorToUpdate = _mapper.Map<UpdateAuthor>(author);

            updateAuthor.ApplyTo(authorToUpdate);

            author = _mapper.Map(authorToUpdate, author);

            if (author.IsValid())
            {
                await _authorRepository.UpdateAsync(author);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                throw new Exception($"Author with id '{id}' not exist");
            }

            await _authorRepository.RemoveAsync(id);
        }
    }
}