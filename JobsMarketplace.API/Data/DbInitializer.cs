using Bogus;
using JobsMarketplace.API.Entities;

namespace JobsMarketplace.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            SeedCustomers(context);
            SeedContractors(context);
        }

        private static void SeedCustomers(AppDbContext context)
        {
            if (context.Customers.Any())
            {
                return;
            }

            var customers = new Customer[]
            {
                new Customer(1, "Leon", "Tayson"),
                new Customer(2, "John", "Doe"),
                new Customer(3, "Will", "Smith"),
                new Customer(5, "Anne Curtis", "Smith"),
                new Customer(4, "Pepe", "Smith"),
            };

            context.Customers.AddRange(customers);
            int nextId = customers.Count() + 1;

            var faker = new Faker<Customer>()
                .CustomInstantiator(f => new Customer(
                    nextId++,
                    f.Name.FirstName(),
                    f.Name.LastName()
                ));

            context.Customers.AddRange(faker.Generate(1000));

            // Test auto-incremment
            // context.Customers.Add(new Customer(Guid.NewGuid().ToString(), "Dummy"));
            context.SaveChanges();
        }

        private static void SeedContractors(AppDbContext context)
        {
            if (context.Contractors.Any())
            {
                return;
            }

            var contractors = new Contractor[]
            {
                    new Contractor(1, "Bob The Builder", 4.9),
                    new Contractor(2, "St. Gerrard Construction", 1.0),
                    new Contractor(3, "Wawao Builders", 1.7),
                    new Contractor(4, "SYMS Construction Trading", 1.3),
                    new Contractor(5, "Bob Marley Construction", 4.5),
                    new Contractor(6, "Bob Ong Construction", 4.0),
            };
            context.Contractors.AddRange(contractors);
            int nextId = contractors.Count() + 1;

            var faker = new Faker<Contractor>()
                .CustomInstantiator(f => new Contractor(
                    nextId++,
                    f.Company.CompanyName(),
                    Math.Round(f.Random.Double(1.0, 5.0), 1)));
            context.Contractors.AddRange(faker.Generate(50));
            context.SaveChanges();
        }
    }
}
