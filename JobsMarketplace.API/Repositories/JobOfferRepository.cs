using JobsMarketplace.API.Data;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace JobsMarketplace.API.Repositories;

public class JobOfferRepository(IAppDbContext context) : IJobOfferRepository
{
    public async Task<JobOfferDto?> GetByIdAsync(int id) =>
    await context.JobOffers
        .Where(o => o.Id == id)
        .Select(o => new JobOfferDto(o.Id, o.JobId, o.ContractorId, o.Amount, o.Message, o.Status))
        .SingleOrDefaultAsync();

    public async Task<List<JobOfferDto>> GetByJobIdAsync(int jobId) =>
        await context.JobOffers
            .Where(o => o.JobId == jobId)
            .Select(o => new JobOfferDto(o.Id, o.JobId, o.ContractorId, o.Amount, o.Message, o.Status))
            .ToListAsync();

    public async Task<bool> OfferExistsAsync(int jobId, int contractorId) =>
        await context.JobOffers.AnyAsync(o => o.JobId == jobId && o.ContractorId == contractorId);

    public async Task AddAsync(JobOffer offer) => await context.JobOffers.AddAsync(offer);

    public async Task<JobOffer?> GetEntityByIdAsync(int id) => await context.JobOffers.FindAsync(id);

    public async Task<Job?> GetJobByIdAsync(int jobId) => await context.Jobs.FindAsync(jobId);

    public void Remove(JobOffer offer) => context.JobOffers.Remove(offer);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();

    public async Task<IDbContextTransaction> BeginTransactionAsync() =>
        await context.Database.BeginTransactionAsync();
}