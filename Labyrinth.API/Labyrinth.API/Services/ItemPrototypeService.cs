using FluentResults;
using Labyrinth.API.Entities.Items;
using Labyrinth.API.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace Labyrinth.API.Services;

public class ItemPrototypeService : IItemPrototypeService
{
    private readonly IMongoCollection<BsonDocument> _itemsCollection;

    public ItemPrototypeService(IMongoDatabase database)
    {
        _itemsCollection = database.GetCollection<BsonDocument>("Items");
    }

    public async Task<Result<Item>> CreateItemPrototypeAsync(Item item)
    {
        try
        {
            await _itemsCollection.InsertOneAsync(item.ToBsonDocument());
            return Result.Ok(item);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Failed to create item prototype: {ex.Message}"));
        }
    }

    public async Task<Result<Item>> GetItemPrototypeByIdAsync(ObjectId id)
    {
        try
        {
            var bsonDocument = await _itemsCollection.Find(new BsonDocument("_id", id)).FirstOrDefaultAsync();
            if (bsonDocument == null)
            {
                return Result.Fail(new Error("Item prototype not found"));
            }

            // Deserialize using the custom deserialization method
            var jsonString = bsonDocument.ToJson();
            var item = ItemDeserializer.DeserializeItems(jsonString).FirstOrDefault();

            return Result.Ok(item);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Failed to retrieve item prototype: {ex.Message}"));
        }
    }


    public async Task<Result<List<Item>>> GetAllItemPrototypesAsync()
    {
        try
        {
            // Fetch all documents as BsonDocuments
            var bsonDocuments = await _itemsCollection.Find(new BsonDocument()).ToListAsync();

            // Use JsonWriterSettings to convert BSON to JSON properly
            var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };

            // Convert each BsonDocument to a JSON string using the specified settings
            var jsonStrings = bsonDocuments.Select(doc => doc.ToJson(jsonWriterSettings)).ToList();

            // Combine JSON strings into a single JSON array string
            var jsonArrayString = $"[{string.Join(",", jsonStrings)}]";

            // Now parse JSON array string to JArray
            var jsonArray = JArray.Parse(jsonArrayString);

            // Deserialize each JSON object to the specific Item type
            var items = jsonArray.Select(obj => DeserializeToSpecificType(obj.ToString())).ToList();

            return Result.Ok(items);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Failed to retrieve item prototypes: {ex.Message}"));
        }
    }

    public async Task<Result> UpdateItemPrototypeAsync(ObjectId id, Item updatedItem)
    {
        try
        {
            var result = await _itemsCollection.ReplaceOneAsync(new BsonDocument("_id", id), updatedItem.ToBsonDocument());
            return result.IsAcknowledged && result.ModifiedCount > 0
                ? Result.Ok()
                : Result.Fail(new Error("Item prototype not found or update failed"));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Failed to update item prototype: {ex.Message}"));
        }
    }

    public async Task<Result> DeleteItemPrototypeAsync(ObjectId id)
    {
        try
        {
            var result = await _itemsCollection.DeleteOneAsync(new BsonDocument("_id", id));
            return result.IsAcknowledged && result.DeletedCount > 0
                ? Result.Ok()
                : Result.Fail(new Error("Item prototype not found or delete failed"));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Failed to delete item prototype: {ex.Message}"));
        }
    }

    // TODO: Move this to utilities static class

    private Item DeserializeToSpecificType(string jsonString)
    {
        var jsonObject = JObject.Parse(jsonString);
        var itemType = Enum.Parse<ItemType>(jsonObject["type"]!.ToString(), true);

        switch (itemType)
        {
            case ItemType.Weapon:
                return jsonObject.ToObject<Weapon>();
            case ItemType.Armor:
                return jsonObject.ToObject<Armor>();
            case ItemType.Consumable:
                return jsonObject.ToObject<Consumable>();
            case ItemType.MagicItem:
                return jsonObject.ToObject<MagicItem>();
            case ItemType.Tool:
                return jsonObject.ToObject<Tool>();
            case ItemType.Treasure:
                return jsonObject.ToObject<Treasure>();
            case ItemType.Container:
                return jsonObject.ToObject<Container>();
            case ItemType.QuestItem:
                return jsonObject.ToObject<QuestItem>();
            default:
                throw new InvalidOperationException($"Unknown item type: {itemType}");
        }
    }
}
