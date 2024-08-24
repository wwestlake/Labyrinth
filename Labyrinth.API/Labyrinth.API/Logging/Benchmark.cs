using System.Diagnostics;

namespace Labyrinth.API.Logging
{

    public class Benchmark<T> : IBenchmark<T>
    {
        private readonly ILogger<T> _logger;
        private Stopwatch _stopwatch;
        private string _name;
        private Dictionary<string, object> _additionalData;

        public Benchmark(ILogger<T> logger)
        {
            _logger = logger;
            _additionalData = new Dictionary<string, object>();
        }

        public void Start(string name)
        {
            _name = name;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Stop(string description = null, Dictionary<string, object> additionalData = null)
        {
            if (_stopwatch == null)
            {
                throw new InvalidOperationException("Benchmark must be started before it can be stopped.");
            }

            _stopwatch.Stop();

            var durationMs = _stopwatch.ElapsedMilliseconds;

            var benchmarkData = new BenchmarkData
            {
                ActionName = _name,
                DurationMs = durationMs,
                AdditionalData = additionalData ?? _additionalData
            };

            _logger.LogInformation($"Benchmark completed: {description ?? _name}", benchmarkData);
        }

        public void AddData(string key, object value)
        {
            _additionalData[key] = value;
        }

        public BenchmarkScope<T> StartScoped(string name)
        {
            Start(name);
            return new BenchmarkScope<T>(this);
        }
    }
}
