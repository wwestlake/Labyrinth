using Labyrinth.API.Entities.Items;
using Labyrinth.API.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Labyrinth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemPrototypesController : ControllerBase
{
    private readonly IItemPrototypeService _itemPrototypeService;

    public ItemPrototypesController(IItemPrototypeService itemPrototypeService)
    {
        _itemPrototypeService = itemPrototypeService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Item item)
    {
        var result = await _itemPrototypeService.CreateItemPrototypeAsync(item);
        return result.IsSuccess ? Ok(result.Value) : Problem(result.Errors.First().Message);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid ID format.");

        var result = await _itemPrototypeService.GetItemPrototypeByIdAsync(objectId);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors.First().Message);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _itemPrototypeService.GetAllItemPrototypesAsync();
        return result.IsSuccess ? Ok(result.Value) : Problem(result.Errors.First().Message);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Item updatedItem)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid ID format.");

        var result = await _itemPrototypeService.UpdateItemPrototypeAsync(objectId, updatedItem);
        return result.IsSuccess ? NoContent() : Problem(result.Errors.First().Message);
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid ID format.");

        var result = await _itemPrototypeService.DeleteItemPrototypeAsync(objectId);
        return result.IsSuccess ? NoContent() : Problem(result.Errors.First().Message);
    }
}
