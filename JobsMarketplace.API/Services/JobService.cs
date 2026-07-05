using JobsMarketplace.API.Constants;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;
using JobsMarketplace.API.Repositories;

namespace JobsMarketplace.API.Services
{
    public class JobService(IJobRepository repository, ICacheService cache) : IJobService
    {
        public async Task<Job> GetById(int id)
        {
            return new Job(id, DateTime.Now, DateTime.Now.AddYears(1), 100, $"Job {id}");
        }

        public async Task<List<JobDto>> GetAvailableJobs(int pageNumber, int pageSize)
        {
            return await repository.GetAvailableJobsAsync(pageNumber, pageSize);
        }

        public async Task<JobDto> Create(JobDto dto)
        {
            var job = new Job(dto.StartDate, dto.DueDate, dto.Budget, dto.Description);
            await repository.AddAsync(job);
            await repository.SaveChangesAsync();

            return new JobDto
            {
                Id = job.Id,
                StartDate = job.StartDate,
                DueDate = job.DueDate,
                Budget = job.Budget,
                Description = job.Description,
                AcceptedById = job.AcceptedById
            };
        }

        public async Task<bool> Update(int id, JobDto dto)
        {
            var job = await repository.GetByIdAsync(id);
            if (job == null) return false;

            job.StartDate = dto.StartDate;
            job.DueDate = dto.DueDate;
            job.Budget = dto.Budget;
            job.Description = dto.Description;
            job.AcceptedById = dto.AcceptedById;

            await repository.SaveChangesAsync();

            // Invalidate cache
            cache.Remove(CacheKeys.Jobs.ById(id));

            return true;
        }

        public async Task<bool> DeleteById(int id)
        {
            var job = await repository.GetByIdAsync(id);
            if (job == null) return false;

            repository.Remove(job);
            await repository.SaveChangesAsync();

            // Remove from cache
            cache.Remove(CacheKeys.Jobs.ById(id));
            return true;
        }
    }
}