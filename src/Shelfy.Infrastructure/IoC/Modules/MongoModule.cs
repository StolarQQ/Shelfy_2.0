using Autofac;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Shelfy.Infrastructure.IoC.Modules
{
    public class MongoModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public MongoModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c, p) =>
            {
                return new MongoClient(_configuration["ConnectionStrings:ShelfyDatabase"]);

            }).SingleInstance();

            builder.Register((c, p) =>
            {
                var client = c.Resolve<MongoClient>();
                var database = client.GetDatabase(_configuration["Mongo:Database"]);

                return database;

            }).As<IMongoDatabase>();
        }
    }
}