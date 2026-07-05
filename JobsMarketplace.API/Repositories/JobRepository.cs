using JobsMarketplace.API.Data;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobsMarketplace.API.Repositories;

public class JobRepository(IAppDbContext context) : IJobRepository
{
    public async Task<Job?> GetByIdAsync(int id) => await context.Jobs.FindAsync(id);

    public async Task<List<JobDto>> GetAvailableJobsAsync(int pageNumber, int pageSize)
    {
        return await context.Jobs
            .Where(j => j.AcceptedBy == null)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(j => new JobDto // Database only selects these columns
            {
                Id = j.Id,
                StartDate = j.StartDate,
                DueDate = j.DueDate,
                Budget = j.Budget,
                Description = j.Description,
                AcceptedById = j.AcceptedById
            })
            .ToListAsync();
    }

    public async Task AddAsync(Job job) => await context.Jobs.AddAsync(job);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();

    public void Remove(Job job)
    {
        context.Jobs.Remove(job);
    }
}
