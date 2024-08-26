using FluentResults;
using Labyrinth.API.Entities.Items;
using MongoDB.Bson;

namespace Labyrinth.API.Services;

public interface IItemPrototypeService
{
    Task<Result<Item>> CreateItemPrototypeAsync(Item item);
    Task<Result<Item>> GetItemPrototypeByIdAsync(ObjectId id);
    Task<Result<List<Item>>> GetAllItemPrototypesAsync();
    Task<Result> UpdateItemPrototypeAsync(ObjectId id, Item updatedItem);
    Task<Result> DeleteItemPrototypeAsync(ObjectId id);
}
