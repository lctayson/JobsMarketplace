using JobsMarketplace.API.Data;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobsMarketplace.API.Services;

public class CustomerService(IAppDbContext context) : ICustomerService
{
    public async Task<List<CustomerDto>> SearchCustomers(string searchTerm, int page = 1, int pageSize = 20)
    {
        var query = context.Customers.AsNoTracking();

        // Prevent SQL injection
        if (int.TryParse(searchTerm, out int id))
        {
            query = query.Where(c => c.Id == id);
        }
        else
        {
            // Use StartsWith for better performance and to avoid SQL injection
            query = query.Where(c => c.LastName.StartsWith(searchTerm));
        }

        return await query.OrderBy(c => c.LastName)
                          .Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .Select(c => new CustomerDto(c.Id, c.FirstName, c.LastName))
                          .ToListAsync();
    }
}
