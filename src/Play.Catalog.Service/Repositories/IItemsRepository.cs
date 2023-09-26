using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
  public interface IItemsRepository
  {
    Task<IReadOnlyCollection<Item>> GetItemsAsync();
    Task<Item> GetItemAsync(Guid id);
    Task CreateItemAsync(Item entity);
    Task UpdateItemAsync(Item entity);
    Task RemoveItemAsync(Guid id);
  }
}