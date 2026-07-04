using FluentAssertions;
using JobsMarketplace.API.Data;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;
using JobsMarketplace.API.Services;
using JobsMarketplace.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace JobsMarketplace.API.UnitTests;

public class JobServiceTests
{
    private readonly Mock<IAppDbContext> _mockContext;
    private readonly Mock<ICacheService> _mockCache;
    private readonly JobService _service;

    public JobServiceTests()
    {
        _mockContext = new Mock<IAppDbContext>();
        _mockCache = new Mock<ICacheService>();
        _service = new JobService(_mockContext.Object, _mockCache.Object);
    }

    [Fact]
    public async Task Create_ShouldAddNewJob_AndReturnDto()
    {
        // Arrange
        var dto = new JobDto { Description = "Test Job", Budget = 100 };
        var mockSet = new Mock<DbSet<Job>>();
        _mockContext.Setup(c => c.Jobs).Returns(mockSet.Object);

        // Act
        var result = await _service.Create(dto);

        // Assert
        result.Description.Should().Be(dto.Description);
        _mockContext.Verify(c => c.Jobs.Add(It.IsAny<Job>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteById_ShouldReturnFalse_WhenJobDoesNotExist()
    {
        // Arrange
        int jobId = 99;
        _mockContext.Setup(c => c.Jobs.FindAsync(jobId)).ReturnsAsync((Job?)null);

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

        _mockContext.Setup(c => c.Jobs.FindAsync(jobId)).ReturnsAsync(existingJob);
        _mockContext.Setup(c => c.Jobs.Remove(existingJob));

        // Act
        var result = await _service.DeleteById(jobId);

        // Assert
        result.Should().BeTrue();
        _mockCache.Verify(c => c.Remove($"job_{jobId}"), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }
}