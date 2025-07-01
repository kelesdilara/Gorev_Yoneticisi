using Microsoft.Extensions.Options;
using MongoDB.Driver;
using piton_taskmanagement_api.Settings;

namespace piton_taskmanagement_api.Context
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }
    }
}
