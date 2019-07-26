using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Shelfy.Core.Domain;
using Shelfy.Core.Helper;
using Shelfy.Core.Repositories;
using Shelfy.Infrastructure.Mongodb;

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
            => await Authors.AsQueryable().FirstOrDefaultAsync(x => x.AuthorId == id);
        
        public async Task<IEnumerable<Author>> BrowseByPhraseAsync(string phrase)
            => await Authors.AsQueryable().Where(x => x.FullName.ToLowerInvariant()
                .Contains(phrase.ToLowerInvariant())).Take(10).ToListAsync();
        
        public async Task<PagedResult<Author>> BrowseAsync(int currentPage, int pageSize)
            => await Authors.AsQueryable().PaginateAsync(currentPage, pageSize);

        public async Task AddAsync(Author author)
            => await Authors.InsertOneAsync(author);
        
        public async Task UpdateAsync(Author author)
            => await Authors.ReplaceOneAsync(x => x.AuthorId == author.AuthorId, author);
        
        public async Task RemoveAsync(Guid id)
            => await Authors.DeleteOneAsync(x => x.AuthorId == id);
        
        private IMongoCollection<Author> Authors => _database.GetCollection<Author>("Authors");
    }
}