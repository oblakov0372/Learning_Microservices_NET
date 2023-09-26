using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{
  [Controller]
  [Route("items")]
  public class ItemsController : ControllerBase
  {
    private readonly IItemsRepository itemsRepository;
    public ItemsController(IItemsRepository itemsRepository)
    {
      this.itemsRepository = itemsRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetAsync()
    {
      var items = (await itemsRepository.GetItemsAsync())
                  .Select(i => i.AsDto());
      return items;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
    {
      var item = await itemsRepository.GetItemAsync(id);
      if (item == null)
        return NotFound();
      return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync([FromBody] CreateItemDto itemDto)
    {
      var item = new Item
      {
        Id = Guid.NewGuid(),
        CreatedDate = DateTimeOffset.Now,
        Name = itemDto.Name,
        Description = itemDto.Description,
        Price = itemDto.Price
      };
      await itemsRepository.CreateItemAsync(item);

      return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(Guid id, [FromBody] UpdateItemDto updateItemDto)
    {
      var existingItem = await itemsRepository.GetItemAsync(id);
      if (existingItem == null)
        return NotFound();

      existingItem.Name = updateItemDto.Name;
      existingItem.Description = updateItemDto.Description;
      existingItem.Price = updateItemDto.Price;

      await itemsRepository.UpdateItemAsync(existingItem);

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
      var existingItem = await itemsRepository.GetItemAsync(id);

      if (existingItem == null)
        return NotFound();

      await itemsRepository.RemoveItemAsync(existingItem.Id);

      return NoContent();
    }
  }

}