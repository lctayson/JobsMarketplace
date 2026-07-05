using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace JobsMarketplace.API.Repositories
{
    public interface IJobOfferRepository
    {
        Task AddAsync(JobOffer offer);

        Task<IDbContextTransaction> BeginTransactionAsync();

        Task<JobOffer?> GetEntityByIdAsync(int id);

        Task<JobOfferDto?> GetByIdAsync(int id);

        Task<List<JobOfferDto>> GetByJobIdAsync(int jobId);

        Task<Job?> GetJobByIdAsync(int jobId);

        Task<bool> OfferExistsAsync(int jobId, int contractorId);

        void Remove(JobOffer offer);

        Task SaveChangesAsync();
    }
}