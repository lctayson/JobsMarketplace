namespace JobsMarketplace.API.Services.Interfaces;

public interface ICacheService
{
    Task<T?> GetOrSet<T>(string key, Func<Task<T>> factory, TimeSpan expiration);
    void Remove(string key);
}