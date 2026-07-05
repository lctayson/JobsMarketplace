using JobsMarketplace.API.Data;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JobsMarketplace.API.Services;

public class CustomerService(ICustomerRepository repository) : ICustomerService
{
    public async Task<List<CustomerDto>> SearchCustomers(string searchTerm, int page = 1, int pageSize = 20)
    {
        return await repository.SearchAsync(searchTerm, page, pageSize);
    }
}
