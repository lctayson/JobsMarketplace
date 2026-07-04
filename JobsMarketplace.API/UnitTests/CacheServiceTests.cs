using FluentAssertions;
using JobsMarketplace.API.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace JobsMarketplace.API.UnitTests;

public class CacheServiceTests
{
    private readonly Mock<IMemoryCache> _mockCache;
    private readonly CacheService _service;

    public CacheServiceTests()
    {
        _mockCache = new Mock<IMemoryCache>();
        _service = new CacheService(_mockCache.Object);
    }

    [Fact]
    public async Task GetOrSet_ShouldReturnCachedValue_WhenKeyExists()
    {
        // Arrange
        string key = "testKey";
        string expectedValue = "cachedData";
        object cacheValue = expectedValue;

        _mockCache.Setup(m => m.TryGetValue(key, out cacheValue)).Returns(true);

        // Act
        var result = await _service.GetOrSet(key, () => Task.FromResult("factoryData"), TimeSpan.FromMinutes(5));

        // Assert
        result.Should().Be(expectedValue);
    }

    [Fact]
    public async Task GetOrSet_ShouldInvokeFactory_WhenKeyDoesNotExist()
    {
        // Arrange
        string key = "testKey";
        string expectedValue = "factoryData";
        object? nullValue = null;

        _mockCache.Setup(m => m.TryGetValue(key, out nullValue)).Returns(false);

        // Setup the CreateEntry method to return a mock ICacheEntry
        _mockCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

        // Act
        var result = await _service.GetOrSet(key, () => Task.FromResult(expectedValue), TimeSpan.FromMinutes(5));

        // Assert
        result.Should().Be(expectedValue);
        _mockCache.Verify(m => m.CreateEntry(key), Times.Once);
    }
}
