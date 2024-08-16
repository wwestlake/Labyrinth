using System;

namespace Labyrinth.API.Logging
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string LogLevel { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        public int EventId { get; set; }
        public string Exception { get; set; }
        public string State { get; set; }
        public BenchmarkData BenchmarkData { get; set; } // Custom benchmark data, if applicable
    }
}
