using Labyrinth.API.Entities.Items;
using Labyrinth.API.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Labyrinth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> GetAllItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(string id)
        {
            var objectId = new ObjectId(id);
            var item = await _itemService.GetItemByIdAsync(objectId);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem(Item newItem)
        {
            await _itemService.CreateItemAsync(newItem);
            return CreatedAtAction(nameof(GetItem), new { id = newItem.Id.ToString() }, newItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] Item updatedItem)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ID format.");
            }

            var existingItem = await _itemService.GetItemByIdAsync(objectId);

            if (existingItem == null)
            {
                return NotFound();
            }

            updatedItem.Id = objectId; // Ensure the existing _id is preserved
            await _itemService.UpdateItemAsync(objectId, updatedItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            var objectId = new ObjectId(id);
            var item = await _itemService.GetItemByIdAsync(objectId);

            if (item == null)
            {
                return NotFound();
            }

            await _itemService.DeleteItemAsync(objectId);
            return NoContent();
        }
    }
}
