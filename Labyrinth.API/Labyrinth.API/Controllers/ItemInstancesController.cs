using Labyrinth.API.Entities.Items.Instance;
using Labyrinth.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Labyrinth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemInstancesController : ControllerBase
{
    private readonly IItemInstanceService _itemInstanceService;

    public ItemInstancesController(IItemInstanceService itemInstanceService)
    {
        _itemInstanceService = itemInstanceService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ItemInstance itemInstance)
    {
        var result = await _itemInstanceService.CreateItemInstanceAsync(itemInstance);
        return result.IsSuccess ? Ok(result.Value) : Problem(result.Errors.First().Message);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid ID format.");

        var result = await _itemInstanceService.GetItemInstanceByIdAsync(objectId);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors.First().Message);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _itemInstanceService.GetAllItemInstancesAsync();
        return result.IsSuccess ? Ok(result.Value) : Problem(result.Errors.First().Message);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, ItemInstance updatedItemInstance)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid ID format.");

        var result = await _itemInstanceService.UpdateItemInstanceAsync(objectId, updatedItemInstance);
        return result.IsSuccess ? NoContent() : Problem(result.Errors.First().Message);
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid ID format.");

        var result = await _itemInstanceService.DeleteItemInstanceAsync(objectId);
        return result.IsSuccess ? NoContent() : Problem(result.Errors.First().Message);
    }
}
