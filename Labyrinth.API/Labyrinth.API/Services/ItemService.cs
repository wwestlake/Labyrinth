using Labyrinth.API.Entities.Items;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Labyrinth.API.Services;

public class ItemService
{
    private readonly IMongoCollection<Item> _items;

    public ItemService(IMongoClient mongoClient, string databaseName, string collectionName)
    {
        var database = mongoClient.GetDatabase(databaseName);
        _items = database.GetCollection<Item>(collectionName);
    }

    public async Task<List<Item>> GetAllItemsAsync()
    {
        return await _items.Find(item => true).ToListAsync();
    }

    public async Task<Item> GetItemByIdAsync(ObjectId id)
    {
        return await _items.Find<Item>(item => item.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Item> CreateItemAsync(Item newItem)
    {
        await _items.InsertOneAsync(newItem);
        return newItem;
    }

    public async Task UpdateItemAsync(ObjectId id, Item updatedItem)
    {
        await _items.ReplaceOneAsync(item => item.Id == id, updatedItem);
    }

    public async Task DeleteItemAsync(ObjectId id)
    {
        await _items.DeleteOneAsync(item => item.Id == id);
    }
}
