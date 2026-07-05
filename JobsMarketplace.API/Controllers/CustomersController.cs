using JobsMarketplace.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobsMarketplace.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpGet("{searchTerm}")]
    public async Task<IActionResult> SearchCustomers(
        string searchTerm,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        return customerService.SearchCustomers(searchTerm, page, pageSize) is { } customers
            ? Ok(customers)
            : NotFound();
    }
}
