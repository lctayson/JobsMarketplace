using JobsMarketplace.API.Dtos;

namespace JobsMarketplace.API.Services.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerDto>> SearchCustomers(string searchTerm, int page = 1, int pageSize = 20);
}