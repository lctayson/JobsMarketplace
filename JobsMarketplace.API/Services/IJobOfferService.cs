using JobsMarketplace.API.Dtos;

namespace JobsMarketplace.API.Services;

public interface IJobOfferService
{
    Task<JobOfferDto?> GetById(int id);

    Task<(bool Success, string ErrorMessage)> Accept(int offerId);

    Task<JobOfferDto?> Create(CreateJobOfferDto dto);

    Task<bool> Delete(int id);

    Task<List<JobOfferDto>> GetByJobId(int jobId);
}