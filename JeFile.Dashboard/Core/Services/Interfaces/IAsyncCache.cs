using System;

namespace JeFile.Dashboard.Core.Services.Interfaces;

public interface IAsyncCache
{
    Task<object?> GetOrAdd(string key, Func<CancellationToken, Task<object?>> loadDelegate, Func<CacheEntryOptions?>? buildOptionsDelegate = null, CancellationToken cancellation = default);
    Task<object?> GetOrDefault(string key, CancellationToken cancellation = default);
    Task AddOrReplace(string key, object? model, CacheEntryOptions? options = null, CancellationToken cancellation = default);
    Task Remove(string key, CancellationToken cancellation = default);
}

public class CacheEntryOptions
{
        public DateTimeOffset? AbsoluteExpiration { get; init; }
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; init; }
        public TimeSpan? SlidingExpiration { get; init; }
}
