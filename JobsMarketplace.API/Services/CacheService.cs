using Microsoft.Extensions.Caching.Memory;

namespace JobsMarketplace.API.Services;

public class CacheService(IMemoryCache cache) : ICacheService
{
    public async Task<T?> GetOrSet<T>(string key, Func<Task<T>> factory, TimeSpan expiration)
    {
        if (cache.TryGetValue(key, out T? value)) return value;

        value = await factory();

        if (value != null)
        {
            cache.Set(key, value, expiration);
        }

        return value;
    }

    public void Remove(string key) => cache.Remove(key);
}
