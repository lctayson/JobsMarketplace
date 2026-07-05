using JobsMarketplace.API.Dtos;

namespace JobsMarketplace.API.Services;

public interface IContractorService
{
    Task<List<ContractorDto>> SearchContractors(string searchTerm, int page = 1, int pageSize = 20);

    Task<bool> DeleteById(int id);
}