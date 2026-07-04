using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Services;
using JobsMarketplace.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobsMarketplace.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractorsController(IContractorService contractorService) : ControllerBase
{
    // GET: api/contractors/Bob
    [HttpGet("{searchTerm}")]
    public async Task<ActionResult<List<ContractorDto>>> Search(
        string searchTerm,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var results = await contractorService.SearchContractors(searchTerm, page, pageSize);
        return results is { } contractors ? Ok(results) : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        // Call the service layer to perform the deletion
        var success = await contractorService.DeleteById(id);

        if (!success)
        {
            // Return 404 if the contractor did not exist
            return NotFound(new { message = $"Contractor with ID {id} not found." });
        }

        // Return 204 No Content upon a successful deletion
        return NoContent();
    }
}
