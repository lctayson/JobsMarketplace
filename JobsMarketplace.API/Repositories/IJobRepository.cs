using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;

namespace JobsMarketplace.API.Repositories
{
    public interface IJobRepository
    {
        Task AddAsync(Job job);

        Task<List<JobDto>> GetAvailableJobsAsync(int pageNumber, int pageSize);

        Task<Job?> GetByIdAsync(int id);

        void Remove(Job job);

        Task SaveChangesAsync();
    }
}