namespace Labyrinth.API.Logging;

public interface IBenchmark<T>
{
    BenchmarkScope<T> StartScoped(string name);
    void AddData(string key, object value);
    void Start(string name);
    void Stop(string description = null, Dictionary<string, object> additionalData = null);
}

