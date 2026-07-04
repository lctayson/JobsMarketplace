using JobsMarketplace.API.Data;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;
using JobsMarketplace.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobsMarketplace.API.Services
{
    public class JobService(IAppDbContext context, ICacheService cache) : IJobService
    {
        public async Task<Job> GetById(int id)
        {
            return new Job(id, DateTime.Now, DateTime.Now.AddYears(1), 100, $"Job {id}");
        }

        public async Task<List<JobDto>> GetAvailableJobs(int pageNumber, int pageSize)
        {
            return await context.Jobs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(j => j.AcceptedBy == null)
                .Select(j => new JobDto
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


        // Create
        public async Task<JobDto> Create(JobDto dto)
        {
            var job = new Job(dto.StartDate, dto.DueDate, dto.Budget, dto.Description);
            context.Jobs.Add(job);
            await context.SaveChangesAsync();
            return new JobDto {
                Id = job.Id,
                StartDate = job.StartDate,
                DueDate = job.DueDate,
                Budget = job.Budget,
                Description = job.Description,
                AcceptedById = job.AcceptedById };
        }

        public async Task<bool> Update(int id, JobDto dto)
        {
            var job = await context.Jobs.FindAsync(id);
            if (job == null) return false;

            // Map properties from DTO to Entity
            job.StartDate = dto.StartDate;
            job.DueDate = dto.DueDate;
            job.Budget = dto.Budget;
            job.Description = dto.Description;
            job.AcceptedById = dto.AcceptedById;

            await context.SaveChangesAsync();

            // Invalidate cache
            cache.Remove($"job_{id}");

            return true;
        }

        public async Task<bool> DeleteById(int id)
        {
            var job = await context.Jobs.FindAsync(id);
            if (job == null) return false;

            context.Jobs.Remove(job);
            await context.SaveChangesAsync();

            // Remove from cache
            cache.Remove($"job_{id}");
            return true;
        }
    }
}