using JobsMarketplace.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace JobsMarketplace.API.Data
{
    public interface IAppDbContext
    {
        DbSet<Contractor> Contractors { get; set; }

        DbSet<Customer> Customers { get; set; }

        DbSet<JobOffer> JobOffers { get; set; }

        DbSet<Job> Jobs { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}