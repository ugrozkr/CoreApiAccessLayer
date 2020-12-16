using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Wall.Discount.Entity;

namespace Wall.Service.DataAccessLayer.DbContexts
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database = null;

        public MongoDbContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Discount.Entity.Discount> Discounts
        {
            get
            {
                return _database.GetCollection<Discount.Entity.Discount>("Discount");
            }
        }
    }
}
