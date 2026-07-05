using JobsMarketplace.API.Data;
using JobsMarketplace.API.Dtos;
using JobsMarketplace.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobsMarketplace.API.Repositories;

public class ContractorRepository(IAppDbContext context) : IContractorRepository
{
    public async Task<List<ContractorDto>> SearchAsync(string searchTerm, int page, int pageSize)
    {
        var query = context.Contractors.AsNoTracking();

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

    public async Task<ContractorDto?> GetByIdAsync(int id)
    {
        return await context.Contractors
            .AsNoTracking()
            .Select(c => new ContractorDto(c.Id, c.Name, c.Rating))
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Contractor?> FindByIdAsync(int id) =>
        await context.Contractors.FindAsync(id);

    public void Remove(Contractor contractor) => context.Contractors.Remove(contractor);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
