using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Shelfy.Core.Domain;
using Shelfy.Core.Repositories;

namespace Shelfy.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository, IMongoRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await Users.AsQueryable().FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await Users.AsQueryable().FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<User>> BrowseAsync(string name)
        {
            return await Users.AsQueryable().ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await Users.InsertOneAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await Users.ReplaceOneAsync(x => x.UserId == user.UserId, user);
        }

        public async Task RemoveAsync(Guid id)
        {
            await Users.DeleteManyAsync(x => x.UserId == id);
        }

        private IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}