using JobsMarketplace.API.Constants;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Repositories;

namespace JobsMarketplace.API.Services;

public class ContractorService(IContractorRepository repository, ICacheService cache) : IContractorService
{
    public async Task<List<ContractorDto>> SearchContractors(
        string searchTerm, int page = 1, int pageSize = 20)
    {
        return await repository.SearchAsync(searchTerm, page, pageSize);
    }

    public async Task<ContractorDto?> GetById(int id)
    {
        return await cache.GetOrSet(
                    CacheKeys.Contractors.ById(id),
                    () => repository.GetByIdAsync(id),
                    TimeSpan.FromMinutes(10)
                );
    }

    public async Task<bool> DeleteById(int id)
    {
        var contractor = await repository.GetEntityByIdAsync(id);
        if (contractor == null) return false;

        repository.Remove(contractor);
        await repository.SaveChangesAsync();

        // Invalidate the cache for this specific record
        cache.Remove(CacheKeys.Contractors.ById(id));
        return true;
    }
}
