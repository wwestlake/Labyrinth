using MongoDB.Driver;

namespace Labyrinth.API.Logging
{
    public class MongoDBLoggerProvider : ILoggerProvider
    {
        private readonly IMongoCollection<LogEntry> _logCollection;

        public MongoDBLoggerProvider(IMongoClient mongoClient, string databaseName, string collectionName)
        {
            var database = mongoClient.GetDatabase(databaseName);
            _logCollection = database.GetCollection<LogEntry>(collectionName);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MongoDBLogger(_logCollection, categoryName);
        }

        public void Dispose()
        {
            // Dispose of any resources if necessary
        }
    }

}
