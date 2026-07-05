using JobsMarketplace.API.Data;
using JobsMarketplace.API.Dtos;
using Microsoft.EntityFrameworkCore;

namespace JobsMarketplace.API.Repositories;

public class CustomerRepository(IAppDbContext context) : ICustomerRepository
{
    public async Task<List<CustomerDto>> SearchAsync(string searchTerm, int page, int pageSize)
    {
        var query = context.Customers.AsNoTracking();

        if (int.TryParse(searchTerm, out int id))
        {
            query = query.Where(c => c.Id == id);
        }
        else
        {
            query = query.Where(c => c.LastName.StartsWith(searchTerm));
        }

        return await query.OrderBy(c => c.LastName)
                          .Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .Select(c => new CustomerDto(c.Id, c.FirstName, c.LastName))
                          .ToListAsync();
    }
}
