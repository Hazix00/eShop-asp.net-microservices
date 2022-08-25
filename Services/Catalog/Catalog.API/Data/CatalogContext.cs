using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }

        public CatalogContext(IConfiguration configuration, ILogger<CatalogContext> logger)
        {
            var conStr = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            var dbName = configuration.GetValue<string>("DatabaseSettings:DatabaseName");
            var collectName = configuration.GetValue<string>("DatabaseSettings:CollectionName");

            var client = new MongoClient(conStr);
            var database = client.GetDatabase(dbName);

            Products = database.GetCollection<Product>(collectName);
            CatalogContextSeed.SeedData(Products);
        }
    }
}
