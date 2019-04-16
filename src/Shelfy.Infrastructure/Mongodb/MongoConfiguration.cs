using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace Shelfy.Infrastructure.Mongodb
{
    public static class MongoConfiguration
    {
        private static bool _initalized;

        public static void Initialize()
        {
            if (_initalized)
            {
                return;
            }

            RegisterConventions();
        }
        
        private static void RegisterConventions()
        {
            ConventionRegistry.Register("ShelfyConventions", new MongoConvention(), x => true);
            _initalized = true;
        }

        private class MongoConvention : IConventionPack
        {
            public IEnumerable<IConvention> Conventions => new List<IConvention>
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention()
            };
        }
    }
}