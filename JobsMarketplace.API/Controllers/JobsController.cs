using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobsMarketplace.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController(IJobService jobService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var job = await jobService.GetById(id);
        return job == null ? NotFound() : Ok(job);
    }

    [HttpGet("available")]
    public async Task<ActionResult<List<JobDto>>> GetAvailableJobs(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        // Just return the list directly
        var items = await jobService.GetAvailableJobs(pageNumber, pageSize);
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<JobDto>> Create([FromBody] JobDto dto)
    {
        var created = await jobService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, JobDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");

        var updated = await jobService.Update(id, dto);

        if (!updated) return NotFound(new { message = $"Job with ID {id} not found." });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        // var deleted = await jobService.DeleteById(id);
        var deleted = await jobService.DeleteById(id);
        return deleted ? NoContent() : NotFound();
    }
}
