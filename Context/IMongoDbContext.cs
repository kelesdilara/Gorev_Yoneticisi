using MongoDB.Driver;

namespace piton_taskmanagement_api.Context
{
    public interface IMongoDbContext
    {
        IMongoDatabase GetDatabase();
    }
}
