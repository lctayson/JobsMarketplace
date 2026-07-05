using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;
using JobsMarketplace.API.Repositories;

namespace JobsMarketplace.API.Services;

public class JobOfferService(IJobOfferRepository repository) : IJobOfferService
{
    public async Task<JobOfferDto?> GetById(int id)
    {
        var offer = await repository.GetEntityByIdAsync(id);

        return offer == null ? null : new JobOfferDto(offer.Id, offer.JobId, offer.ContractorId, offer.Amount, offer.Message, offer.Status);
    }

    public async Task<JobOfferDto?> Create(CreateJobOfferDto dto)
    {
        if (await repository.OfferExistsAsync(dto.JobId, dto.ContractorId)) return null;

        var offer = new JobOffer
        {
            JobId = dto.JobId,
            ContractorId = dto.ContractorId,
            Amount = dto.Amount,
            Message = dto.Message,
            Status = "Pending"
        };

        await repository.AddAsync(offer);
        await repository.SaveChangesAsync();

        // return await repository.GetByIdAsync(offer.Id);
        return new JobOfferDto(offer.Id, offer.JobId, offer.ContractorId, offer.Amount, offer.Message, offer.Status);
    }

    public async Task<List<JobOfferDto>> GetByJobId(int jobId)
    {
        return await repository.GetByJobIdAsync(jobId);
    }

    public async Task<(bool Success, string ErrorMessage)> Accept(int offerId)
    {
        using var transaction = await repository.BeginTransactionAsync();

        var offer = await repository.GetEntityByIdAsync(offerId);
        if (offer == null)
            return (false, "Offer not found.");

        var job = await repository.GetJobByIdAsync(offer.JobId);
        if (job == null)
            return (false, "Job not found.");

        if (job.AcceptedById != null)
            return (false, "Job already taken.");

        offer.Status = "Accepted";
        job.AcceptedById = offer.ContractorId;

        await repository.SaveChangesAsync();
        await transaction.CommitAsync();

        return (true, string.Empty);
    }

    public async Task<bool> Delete(int id)
    {
        var offer = await repository.GetEntityByIdAsync(id);
        if (offer == null || offer.Status == "Accepted") return false;  // Cannot delete accepted offers

        repository.Remove(offer);
        await repository.SaveChangesAsync();
        return true;
    }
}
