using FluentAssertions;
using JobsMarketplace.API.Constants;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;
using JobsMarketplace.API.Repositories;
using JobsMarketplace.API.Services;
using Moq;
using Xunit;

namespace JobsMarketplace.API.UnitTests;

public class JobServiceTests
{
    private readonly Mock<IJobRepository> _mockJobRepository;
    private readonly Mock<ICacheService> _mockCache;
    private readonly JobService _service;

    public JobServiceTests()
    {
        _mockJobRepository = new Mock<IJobRepository>();
        _mockCache = new Mock<ICacheService>();
        _service = new JobService(_mockJobRepository.Object, _mockCache.Object);
    }

    [Fact]
    public async Task Create_ShouldAddNewJob_AndReturnDto()
    {
        // Arrange
        var dto = new JobDto { Description = "Test Job", Budget = 100 };

        // Act
        var result = await _service.Create(dto);

        // Assert
        result.Description.Should().Be(dto.Description);
        _mockJobRepository.Verify(r => r.AddAsync(It.IsAny<Job>()), Times.Once);
        _mockJobRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteById_ShouldReturnFalse_WhenJobDoesNotExist()
    {
        // Arrange
        int jobId = 99;
        _mockJobRepository.Setup(r => r.GetByIdAsync(jobId)).ReturnsAsync((Job?)null);

        // Act
        var result = await _service.DeleteById(jobId);

        // Assert
        result.Should().BeFalse();
        _mockCache.Verify(c => c.Remove(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task DeleteById_ShouldRemoveJob_AndInvalidateCache_WhenJobExists()
    {
        // Arrange
        int jobId = 1;
        var existingJob = new Job(DateTime.Now, DateTime.Now.AddDays(1), 100, "Old Job");

        _mockJobRepository.Setup(r => r.GetByIdAsync(jobId)).ReturnsAsync(existingJob);

        // Act
        var result = await _service.DeleteById(jobId);

        // Assert
        result.Should().BeTrue();

        _mockCache.Verify(c => c.Remove(CacheKeys.Jobs.ById(jobId)), Times.Once);
        _mockJobRepository.Verify(r => r.Remove(existingJob), Times.Once);
        _mockJobRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
