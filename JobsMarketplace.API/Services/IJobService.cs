using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;

namespace JobsMarketplace.API.Services;

public interface IJobService
{
    Task<Job> GetById(int id);

    Task<JobDto> Create(JobDto dto);

    Task<bool> Update(int id, JobDto dto);

    Task<bool> DeleteById(int id);

    Task<List<JobDto>> GetAvailableJobs(int pageNumber, int pageSize);
}
