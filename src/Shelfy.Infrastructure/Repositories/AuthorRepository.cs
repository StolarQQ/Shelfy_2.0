using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;

namespace Shelfy.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository, IMongoRepository
    {
        private readonly IMongoDatabase _database;

        public AuthorRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Author> GetByIdAsync(Guid id)
        {
            return await Authors.AsQueryable().FirstOrDefaultAsync(x => x.AuthorId == id);
        }

        public async Task<IEnumerable<Author>> BrowseByPhraseAsync(string phrase)
        {
            return await Authors.AsQueryable().Where(x => x.FullName.ToLowerInvariant()
                .Contains(phrase.ToLowerInvariant())).Take(10).ToListAsync();
        }

        public async Task<IEnumerable<Author>> BrowseAsync()
        {
            return await Authors.AsQueryable().ToListAsync();
        }

        public async Task AddAsync(Author author)
        {
            await Authors.InsertOneAsync(author);
        }

        public async Task UpdateAsync(Author author)
        {
            await Authors.ReplaceOneAsync(x => x.AuthorId == author.AuthorId, author);
        }

        public async Task RemoveAsync(Guid id)
        {
            await Authors.DeleteOneAsync(x => x.AuthorId == id);
        }

        private IMongoCollection<Author> Authors => _database.GetCollection<Author>("Authors");
    }
}