using System;

namespace Labyrinth.API.Logging
{
    public class BenchmarkData
    {
        public string ActionName { get; set; }
        public long DurationMs { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }
    }
}
