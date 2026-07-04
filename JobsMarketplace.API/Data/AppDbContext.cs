using JobsMarketplace.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobsMarketplace.API.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Contractor> Contractors { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<JobOffer> JobOffers { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite("Data Source=jobsmarketplace.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Configurations & Indexes ---

            // Customers
            modelBuilder.Entity<Customer>()
                .Property(c => c.FirstName).UseCollation("NOCASE");
            modelBuilder.Entity<Customer>()
                .Property(c => c.LastName).UseCollation("NOCASE");
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.LastName);

            // Contractors
            modelBuilder.Entity<Contractor>()
                .Property(c => c.Name).UseCollation("NOCASE");
            modelBuilder.Entity<Contractor>()
                .HasIndex(c => c.Name);

            // JobOffer Relationships
            modelBuilder.Entity<JobOffer>()
                .HasIndex(jo => jo.JobId);

            /*
            // --- Indexes for searching ---

            // Performance: GET /customers/tayson
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.LastName);

            // Performance: Searching contractors by name
            modelBuilder.Entity<Contractor>()
                .HasIndex(c => c.Name);

            modelBuilder.Entity<JobOffer>()
                .HasIndex(jo => jo.JobId);

            //modelBuilder.Entity<Job>()
            //    .HasIndex(j => j.Id);

            //modelBuilder.Entity<Job>()
            //    .HasIndex(j => j.Id);
            */
        }
    }
}
