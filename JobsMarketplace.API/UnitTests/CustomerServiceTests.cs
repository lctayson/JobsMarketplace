
using FluentAssertions;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Repositories;
using JobsMarketplace.API.Services;
using Moq;
using Xunit;

namespace JobsMarketplace.API.UnitTests;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _mockRepository;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _mockRepository = new Mock<ICustomerRepository>();
        _service = new CustomerService(_mockRepository.Object);
    }

    [Fact]
    public async Task SearchCustomers_ShouldReturnListFromRepository_WhenSearchTermIsProvided()
    {
        // Arrange
        var searchTerm = "Doe";
        var page = 1;
        var pageSize = 10;
        var expectedCustomers = new List<CustomerDto>
        {
            new(1, "John", "Doe"),
            new(2, "Jane", "Doe")
        };

        _mockRepository.Setup(r => r.SearchAsync(searchTerm, page, pageSize))
                       .ReturnsAsync(expectedCustomers);

        // Act
        var result = await _service.SearchCustomers(searchTerm, page, pageSize);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedCustomers);

        // Verify that the repository's SearchAsync method was called with the correct parameters
        _mockRepository.Verify(r => r.SearchAsync(searchTerm, page, pageSize), Times.Once);
    }

    [Fact]
    public async Task SearchCustomers_ShouldHandleEmptyResults_WhenNoCustomersMatch()
    {
        // Arrange
        var searchTerm = "NonExistent";
        _mockRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<int>(), It.IsAny<int>()))
                       .ReturnsAsync(new List<CustomerDto>());

        // Act
        var result = await _service.SearchCustomers(searchTerm);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
