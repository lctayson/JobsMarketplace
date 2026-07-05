using Moq;
using FluentAssertions;
using Xunit;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Repositories;
using JobsMarketplace.API.Services;
using JobsMarketplace.API.Constants;
using JobsMarketplace.API.Entities;

namespace JobsMarketplace.API.UnitTests;

public class ContractorServiceTests
{
    private readonly Mock<IContractorRepository> _mockRepo;
    private readonly Mock<ICacheService> _mockCache;
    private readonly ContractorService _service;

    public ContractorServiceTests()
    {
        _mockRepo = new Mock<IContractorRepository>();
        _mockCache = new Mock<ICacheService>();
        _service = new ContractorService(_mockRepo.Object, _mockCache.Object);
    }

    [Fact]
    public async Task SearchContractors_ShouldCallRepository_WithCorrectParameters()
    {
        // Arrange
        var searchTerm = "John";
        var expected = new List<ContractorDto> { new(1, "John Doe", 5) };
        _mockRepo.Setup(r => r.SearchAsync(searchTerm, 1, 20)).ReturnsAsync(expected);

        // Act
        var result = await _service.SearchContractors(searchTerm);

        // Assert
        result.Should().BeEquivalentTo(expected);
        _mockRepo.Verify(r => r.SearchAsync(searchTerm, 1, 20), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldUseCache_WhenCalled()
    {
        // Arrange
        int id = 1;
        var expected = new ContractorDto(id, "Leon King", 5);

        // Setup the cache to return the expected value when GetOrSet is called
        _mockCache.Setup(c => c.GetOrSet(
            CacheKeys.Contractors.ById(id),
            It.IsAny<Func<Task<ContractorDto?>>>(),
            It.IsAny<TimeSpan>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.GetById(id);

        // Assert
        result.Should().BeEquivalentTo(expected);
        _mockCache.Verify(c => c.GetOrSet(CacheKeys.Contractors.ById(id), It.IsAny<Func<Task<ContractorDto?>>>(), It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task DeleteById_ShouldRemoveFromRepo_AndInvalidateCache_WhenExists()
    {
        // Arrange
        int id = 1;
        var contractor = new Contractor("Leon The Builder", 4.9);
        _mockRepo.Setup(r => r.FindByIdAsync(id)).ReturnsAsync(contractor);

        // Act
        var result = await _service.DeleteById(id);

        // Assert
        result.Should().BeTrue();
        _mockRepo.Verify(r => r.Remove(contractor), Times.Once);
        _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mockCache.Verify(c => c.Remove(CacheKeys.Contractors.ById(id)), Times.Once);
    }

    [Fact]
    public async Task DeleteById_ShouldReturnFalse_WhenContractorNotFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((Contractor?)null);

        // Act
        var result = await _service.DeleteById(99);

        // Assert
        result.Should().BeFalse();
        _mockRepo.Verify(r => r.Remove(It.IsAny<Contractor>()), Times.Never);
        _mockCache.Verify(c => c.Remove(It.IsAny<string>()), Times.Never);
    }
}