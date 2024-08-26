using FluentResults;
using Labyrinth.API.Entities.Items.Instance;
using MongoDB.Bson;

namespace Labyrinth.API.Services;

public interface IItemInstanceService
{
    Task<Result<ItemInstance>> CreateItemInstanceAsync(ItemInstance itemInstance);
    Task<Result<ItemInstance>> GetItemInstanceByIdAsync(ObjectId id);
    Task<Result<IEnumerable<ItemInstance>>> GetAllItemInstancesAsync();
    Task<Result> UpdateItemInstanceAsync(ObjectId id, ItemInstance updatedItemInstance);
    Task<Result> DeleteItemInstanceAsync(ObjectId id);
}
