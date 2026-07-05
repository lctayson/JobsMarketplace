namespace JobsMarketplace.API.Services;

public interface ICacheService
{
    Task<T?> GetOrSet<T>(string key, Func<Task<T>> factory, TimeSpan expiration);
    void Remove(string key);
}