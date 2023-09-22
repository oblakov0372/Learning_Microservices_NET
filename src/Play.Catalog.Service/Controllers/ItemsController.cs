using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
  [Controller]
  [Route("items")]
  public class ItemsController : ControllerBase
  {
    private static readonly List<ItemDto> items = new()
    {
      new ItemDto(Guid.NewGuid(),"Potion","Some text here",6,DateTimeOffset.UtcNow),
      new ItemDto(Guid.NewGuid(),"Antidot","Cures Poins",4,DateTimeOffset.UtcNow),
      new ItemDto(Guid.NewGuid(),"Bronze sword","Deals a small amount of damage",8,DateTimeOffset.UtcNow)
    };

    [HttpGet]
    public IEnumerable<ItemDto> Get()
    {
      return items;
    }

    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetById(Guid id)
    {
      var item = items.Where(i => i.Id == id).SingleOrDefault();
      if (item == null)
        return NotFound();
      return Ok(item);
    }

    [HttpPost]
    public ActionResult<ItemDto> Post(CreateItemDto itemDto)
    {
      var item = new ItemDto(Guid.NewGuid(), itemDto.Name, itemDto.Description, itemDto.Price, DateTimeOffset.Now);
      items.Add(item);
      return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }
    [HttpPut("{id}")]
    public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
    {
      var itemForEdit = items.Where(i => i.Id == id).SingleOrDefault();
      if (itemForEdit == null)
        return NotFound();

      var updateItem = itemForEdit with
      {
        Name = updateItemDto.Name,
        Description = updateItemDto.Description,
        Price = updateItemDto.Price
      };

      var index = items.FindIndex(i => i.Id == id);
      items[index] = updateItem;
      return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
      var index = items.FindIndex(i => i.Id == id);
      if (index < 0)
        return NotFound();

      items.RemoveAt(index);
      return NoContent();
    }
  }

}