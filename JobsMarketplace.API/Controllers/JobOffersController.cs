using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobsMarketplace.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobOffersController(IJobOfferService jobOfferService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<JobOfferDto>> Get(int id)
    {
        var offer = await jobOfferService.GetById(id);
        return offer == null ? NotFound() : Ok(offer);
    }

    [HttpPost("{id}/accept")]
    public async Task<IActionResult> Accept(int id)
    {
        var success = await jobOfferService.Accept(id);
        return success ? NoContent() : NotFound("Offer not found or job already taken.");
    }

    [HttpPost]
    public async Task<ActionResult<JobOfferDto>> Create(CreateJobOfferDto dto)
    {
        var offer = await jobOfferService.Create(dto);
        return offer == null ? BadRequest("Offer already exists.") : CreatedAtAction(nameof(GetByJob), new { jobId = dto.JobId }, offer);
    }

    [HttpGet("job/{jobId}")]
    public async Task<ActionResult<List<JobOfferDto>>> GetByJob(int jobId) => Ok(await jobOfferService.GetByJobId(jobId));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await jobOfferService.Delete(id);
        return success ? NoContent() : BadRequest("Cannot delete an accepted offer or offer not found.");
    }
}
