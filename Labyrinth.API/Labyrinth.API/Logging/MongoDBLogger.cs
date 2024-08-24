using MongoDB.Driver;

namespace Labyrinth.API.Logging
{
    public class MongoDBLogger : ILogger
    {
        private readonly IMongoCollection<LogEntry> _logCollection;
        private readonly string _categoryName;

        public MongoDBLogger(IMongoCollection<LogEntry> logCollection, string categoryName)
        {
            _logCollection = logCollection;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true; // Adjust this to filter by log level

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var logEntry = new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                LogLevel = logLevel.ToString(),
                Category = _categoryName,
                Message = formatter(state, exception),
                EventId = eventId.Id,
                Exception = exception?.ToString(),
                State = state.ToString(), // You can serialize state if it's a complex object
                BenchmarkData = state as BenchmarkData // Custom benchmark data if passed in state
            };

            _logCollection.InsertOneAsync(logEntry).Wait(); // Save to MongoDB
        }
    }
}
