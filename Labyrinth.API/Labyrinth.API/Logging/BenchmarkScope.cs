using Labyrinth.API.Logging;

public class BenchmarkScope<T> : IDisposable
{
    private readonly IBenchmark<T> _benchmark;

    public BenchmarkScope(IBenchmark<T> benchmark)
    {
        _benchmark = benchmark;
    }

    public void Dispose()
    {
        _benchmark.Stop();
    }
}
