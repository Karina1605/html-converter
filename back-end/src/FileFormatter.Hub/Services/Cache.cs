using FileFormatter.HubRunner.Abstractions;
using System.Collections.Concurrent;

namespace FileFormatter.HubRunner.Services;

public class Cache : ICache
{
    private readonly ConcurrentDictionary<Guid, string> _cache = new();

    public void AddConnection(Guid requestId, string connectionId)
    {
        _cache.TryAdd(requestId, connectionId);
    }

    public string GetConnection(Guid requestId)
    {
        if (_cache.TryGetValue(requestId, out var connection))
        {
            return connection;
        }

        return string.Empty;
    }
}
