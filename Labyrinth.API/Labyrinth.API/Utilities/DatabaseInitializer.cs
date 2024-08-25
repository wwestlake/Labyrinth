using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Reflection;

namespace Labyrinth.API.Utilities
{
    public static class DatabaseInitializer
    {
        // List of import configurations
        private static readonly List<(string filename, Func<IMongoCollection<BsonDocument>, bool> shouldImport, string)> importFiles = new()
        {
            ("Properties/InitialItems.json", ShouldImportInitialItems, "Items"),
            ("Properties/InitialRooms.json", ShouldImportInitialRooms, "Rooms"),
            ("Properties/The Journey of the Lost Artifact.json", ShouldImportQuests, "Quests")
            // Add more files and conditions here as needed
        };

        // Method to initialize the database
        public static void InitializeDatabase(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Labyrinth");

            foreach (var (filename, shouldImport, collectionName) in importFiles)
            {
                var collection = database.GetCollection<BsonDocument>(collectionName);
                if (shouldImport(collection))
                {
                    ImportJsonFile(filename, collection);
                }
            }
        }

        // Function to check if initial items should be imported
        private static bool ShouldImportInitialItems(IMongoCollection<BsonDocument> itemsCollection)
        {
            // Check if there are any documents in the 'Items' collection
            return !itemsCollection.Find(new BsonDocument()).Any();
        }

        // Function to check if initial rooms should be imported
        private static bool ShouldImportInitialRooms(IMongoCollection<BsonDocument> questCollection)
        {
            // Check if there are any documents in the 'Items' collection
            return !questCollection.Find(new BsonDocument()).Any();
        }

        private static bool ShouldImportQuests(IMongoCollection<BsonDocument> roomsCollection)
        {
            // Check if there are any documents in the 'Items' collection
            return !roomsCollection.Find(new BsonDocument()).Any();
        }

        // Function to import JSON file into MongoDB
        private static void ImportJsonFile(string filename, IMongoCollection<BsonDocument> collection)
        {
            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, filename);
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var documents = BsonSerializer.Deserialize<BsonArray>(json);

                foreach (var document in documents)
                {
                    collection.InsertOne(document.AsBsonDocument);
                }

                Console.WriteLine($"Imported items from {filename} into MongoDB.");
            }
            else
            {
                Console.WriteLine($"File {filename} not found.");
            }
        }
    }
}
