using FluentResults;
using Labyrinth.API.Entities.Items.Instance;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Labyrinth.API.Services;

public class ItemInstanceService : IItemInstanceService
{
    private readonly IMongoCollection<ItemInstance> _itemInstancesCollection;

    public ItemInstanceService(IMongoDatabase database)
    {
        _itemInstancesCollection = database.GetCollection<ItemInstance>("Items");
    }

    public async Task<Result<ItemInstance>> CreateItemInstanceAsync(ItemInstance itemInstance)
    {
        try
        {
            await _itemInstancesCollection.InsertOneAsync(itemInstance);
            return Result.Ok(itemInstance);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Failed to create item instance: {ex.Message}"));
        }
    }

    public async Task<Result<ItemInstance>> GetItemInstanceByIdAsync(ObjectId id)
    {
        try
        {
            var itemInstance = await _itemInstancesCollection.Find(ii => ii.Id == id).FirstOrDefaultAsync();
            return itemInstance != null ? Result.Ok(itemInstance) : Result.Fail(new Error("Item instance not found"));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Failed to retrieve item instance: {ex.Message}"));
        }
    }

    public async Task<Result<IEnumerable<ItemInstance>>> GetAllItemInstancesAsync()
    {
        try
        {
            var itemInstances = await _itemInstancesCollection.Find(_ => true).ToListAsync();
            return Result.Ok(itemInstances.AsEnumerable());
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Failed to retrieve item instances: {ex.Message}"));
        }
    }

    public async Task<Result> UpdateItemInstanceAsync(ObjectId id, ItemInstance updatedItemInstance)
    {
        try
        {
            var result = await _itemInstancesCollection.ReplaceOneAsync(ii => ii.Id == id, updatedItemInstance);
            return result.IsAcknowledged && result.ModifiedCount > 0
                ? Result.Ok()
                : Result.Fail(new Error("Item instance not found or update failed"));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Failed to update item instance: {ex.Message}"));
        }
    }

    public async Task<Result> DeleteItemInstanceAsync(ObjectId id)
    {
        try
        {
            var result = await _itemInstancesCollection.DeleteOneAsync(ii => ii.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0
                ? Result.Ok()
                : Result.Fail(new Error("Item instance not found or delete failed"));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Failed to delete item instance: {ex.Message}"));
        }
    }
}
