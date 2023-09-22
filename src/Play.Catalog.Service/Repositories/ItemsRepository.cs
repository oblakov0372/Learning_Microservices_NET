using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
  public class ItemsRepository
  {
    private const string collectionName = "items";
    private readonly IMongoCollection<Item> dbCollection;
    private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

    public ItemsRepository()
    {
      var mongoClient = new MongoClient("mongodb://localhost:27017");
      var datebase = mongoClient.GetDatabase("Catalog");
      dbCollection = datebase.GetCollection<Item>(collectionName);
    }

    public async Task<IReadOnlyCollection<Item>> GetItemsAsync()
    {
      return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }

    public async Task<Item> GetItemAsync(Guid id)
    {
      FilterDefinition<Item> filter = filterBuilder.Eq(i => i.Id, id);
      return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }
    public async Task CreateItemAsync(Item entity)
    {
      if (entity == null)
        throw new ArgumentException(nameof(entity));
      await dbCollection.InsertOneAsync(entity);
    }
    public async Task UpdateItemAsync(Item entity)
    {
      if (entity == null)
        throw new ArgumentException(nameof(entity));

      FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, entity.Id);
      await dbCollection.ReplaceOneAsync(filter, entity);
    }

    public async Task RemoveItemAsync(Guid id)
    {
      FilterDefinition<Item> filter = filterBuilder.Eq(i => i.Id, id);
      await dbCollection.DeleteOneAsync(filter);
    }

  }
}