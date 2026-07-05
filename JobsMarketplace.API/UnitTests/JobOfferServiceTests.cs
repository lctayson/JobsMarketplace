using Moq;
using FluentAssertions;
using Xunit;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;
using JobsMarketplace.API.Repositories;
using JobsMarketplace.API.Services;
using Microsoft.EntityFrameworkCore.Storage;

namespace JobsMarketplace.API.UnitTests;

public class JobOfferServiceTests
{
    private readonly Mock<IJobOfferRepository> _mockRepo;
    private readonly JobOfferService _service;

    public JobOfferServiceTests()
    {
        _mockRepo = new Mock<IJobOfferRepository>();
        _service = new JobOfferService(_mockRepo.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnNull_WhenOfferAlreadyExists()
    {
        // Arrange
        var dto = new CreateJobOfferDto(1, 1, 100, "Mabuhay");
        _mockRepo.Setup(r => r.OfferExistsAsync(dto.JobId, dto.ContractorId)).ReturnsAsync(true);

        // Act
        var result = await _service.Create(dto);

        // Assert
        result.Should().BeNull();
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<JobOffer>()), Times.Never);
    }

    [Fact]
    public async Task Accept_ShouldCommitTransaction_WhenJobAndOfferExist()
    {
        // Arrange
        int offerId = 1;
        var offer = new JobOffer { Id = offerId, JobId = 10, ContractorId = 5 };
        var job = new Job(DateTime.Now, DateTime.Now.AddMonths(6), 67, "Build build build");

        var mockTransaction = new Mock<IDbContextTransaction>();
        _mockRepo.Setup(r => r.BeginTransactionAsync()).ReturnsAsync(mockTransaction.Object);
        _mockRepo.Setup(r => r.GetEntityByIdAsync(offerId)).ReturnsAsync(offer);
        _mockRepo.Setup(r => r.GetJobByIdAsync(offer.JobId)).ReturnsAsync(job);

        // Act
        var result = await _service.Accept(offerId);

        // Assert
        result.Success.Should().BeTrue();
        offer.Status.Should().Be("Accepted");
        job.AcceptedById.Should().Be(offer.ContractorId);
        mockTransaction.Verify(t => t.CommitAsync(default), Times.Once);
        _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Accept_ShouldReturnFailure_WhenOfferDoesNotExist()
    {
        // Arrange
        int offerId = 99;
        _mockRepo.Setup(r => r.GetEntityByIdAsync(offerId)).ReturnsAsync((JobOffer?)null);

        // Act
        var (success, errorMessage) = await _service.Accept(offerId);

        // Assert
        success.Should().BeFalse();
        errorMessage.Should().Be("Offer not found.");
    }

    [Fact]
    public async Task Accept_ShouldReturnFailure_WhenJobDoesNotExist()
    {
        // Arrange
        int offerId = 1;
        var offer = new JobOffer { Id = offerId, JobId = 10 };
        _mockRepo.Setup(r => r.GetEntityByIdAsync(offerId)).ReturnsAsync(offer);
        _mockRepo.Setup(r => r.GetJobByIdAsync(offer.JobId)).ReturnsAsync((Job?)null);

        // Act
        var (success, errorMessage) = await _service.Accept(offerId);

        // Assert
        success.Should().BeFalse();
        errorMessage.Should().Be("Job not found.");
    }

    [Fact]
    public async Task Accept_ShouldReturnFailure_WhenJobIsAlreadyTaken()
    {
        // Arrange
        int offerId = 1;
        var offer = new JobOffer { Id = offerId, JobId = 10 };
        var job = new Job(1, DateTime.Now, DateTime.Now.AddMonths(3), 999, "Taken for Granted");
        job.AcceptedById = 67;  // Sim job already taken

        _mockRepo.Setup(r => r.GetEntityByIdAsync(offerId)).ReturnsAsync(offer);
        _mockRepo.Setup(r => r.GetJobByIdAsync(offer.JobId)).ReturnsAsync(job);

        // Act
        var (success, errorMessage) = await _service.Accept(offerId);

        // Assert
        success.Should().BeFalse();
        errorMessage.Should().Be("Job already taken.");
    }

    [Fact]
    public async Task Delete_ShouldReturnFalse_WhenOfferIsAccepted()
    {
        // Arrange
        var offer = new JobOffer { Id = 1, Status = "Accepted" };
        _mockRepo.Setup(r => r.GetEntityByIdAsync(1)).ReturnsAsync(offer);

        // Act
        var result = await _service.Delete(1);

        // Assert
        result.Should().BeFalse();
        _mockRepo.Verify(r => r.Remove(It.IsAny<JobOffer>()), Times.Never);
    }
}
