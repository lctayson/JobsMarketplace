using JobsMarketplace.API.Data;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobsMarketplace.API.Services;

public class JobOfferService(IAppDbContext context) : IJobOfferService
{
    public async Task<JobOfferDto?> GetById(int id)
    {
        var offer = await context.JobOffers
            .FirstOrDefaultAsync(o => o.Id == id);

        return offer == null ? null : new JobOfferDto(offer.Id, offer.JobId, offer.ContractorId, offer.Amount, offer.Message, offer.Status);
    }

    public async Task<JobOfferDto?> Create(CreateJobOfferDto dto)
    {
        // Prevent duplicate offers
        var exists = await context.JobOffers.AnyAsync(o => o.JobId == dto.JobId && o.ContractorId == dto.ContractorId);
        if (exists) return null;

        var offer = new JobOffer
        {
            JobId = dto.JobId,
            ContractorId = dto.ContractorId,
            Amount = dto.Amount,
            Message = dto.Message,
            Status = "Pending"
        };

        context.JobOffers.Add(offer);
        await context.SaveChangesAsync();

        return new JobOfferDto(offer.Id, offer.JobId, offer.ContractorId, offer.Amount, offer.Message, offer.Status);
    }

    public async Task<List<JobOfferDto>> GetByJobId(int jobId)
    {
        return await context.JobOffers
            .Where(o => o.JobId == jobId)
            .Select(o => new JobOfferDto(o.Id, o.JobId, o.ContractorId, o.Amount, o.Message, o.Status))
            .ToListAsync();
    }

    public async Task<bool> Accept(int offerId)
    {
        // Use a transaction to ensure atomicity
        using var transaction = await context.Database.BeginTransactionAsync();

        var offer = await context.JobOffers.FindAsync(offerId);
        if (offer == null) return false;

        var job = await context.Jobs.FindAsync(offer.JobId);
        if (job == null) return false;

        offer.Status = "Accepted";
        job.AcceptedById = offer.ContractorId;

        await context.SaveChangesAsync();
        await transaction.CommitAsync();

        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var offer = await context.JobOffers.FindAsync(id);
        if (offer == null || offer.Status == "Accepted") return false; // Cannot delete accepted offers

        context.JobOffers.Remove(offer);
        await context.SaveChangesAsync();
        return true;
    }
}
