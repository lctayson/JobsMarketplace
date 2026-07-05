using JobsMarketplace.API.Data;
using JobsMarketplace.API.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace JobsMarketplace.API.Services;

public class ContractorService(IAppDbContext context, ICacheService cacheService) : IContractorService
{
    public async Task<List<ContractorDto>> SearchContractors(
        string searchTerm, int page = 1, int pageSize = 20)
    {
        var query = context.Contractors.AsNoTracking();

        // Prevent SQL injection
        if (int.TryParse(searchTerm, out int id))
        {
            query = query.Where(c => c.Id == id);
        }
        else
        {
            // Use EF.Functions.Collate to perform a case-insensitive search
            // query = query.Where(c => EF.Functions.Collate(c.Name, "NOCASE").Contains(searchTerm));
            var lowerSearch = searchTerm.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(lowerSearch));
        }

        return await query.OrderBy(c => c.Name)
                          .Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .Select(c => new ContractorDto(c.Id, c.Name, c.Rating))
                          .ToListAsync();
    }

    public async Task<ContractorDto?> GetById(int id)
    {
        return await cacheService.GetOrSet(
            $"contractor_{id}",
            () => context.Contractors
                          .AsNoTracking()
                          .Select(c => new ContractorDto(c.Id, c.Name, c.Rating))
                          .FirstOrDefaultAsync(c => c.Id == id),
            TimeSpan.FromMinutes(10)
        );
    }

    public async Task<bool> DeleteById(int id)
    {
        var contractor = await context.Contractors.FindAsync(id);

        if (contractor == null) return false;

        // Remove and Save
        context.Contractors.Remove(contractor);
        await context.SaveChangesAsync();

        // Invalidate the cache for this specific record
        cacheService.Remove($"contractor_{id}");

        return true;
    }
}
