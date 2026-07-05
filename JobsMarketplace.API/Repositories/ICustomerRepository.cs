using JobsMarketplace.API.Dtos;

namespace JobsMarketplace.API.Repositories;

public interface ICustomerRepository
{
    Task<List<CustomerDto>> SearchAsync(string searchTerm, int page, int pageSize);
}
