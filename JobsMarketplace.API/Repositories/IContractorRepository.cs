using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;

namespace JobsMarketplace.API.Repositories;

public interface IContractorRepository
{
    Task<ContractorDto?> GetByIdAsync(int id);

    Task<Contractor?> GetEntityByIdAsync(int id);

    void Remove(Contractor contractor);

    Task SaveChangesAsync();

    Task<List<ContractorDto>> SearchAsync(string searchTerm, int page, int pageSize);
}