using JobsMarketplace.API.Data;
using JobsMarketplace.API.Services;
using JobsMarketplace.API.Validators;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using JobsMarketplace.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Service registrations
builder.Services.AddScoped<IJobService, JobService>();

// Get the connection string from appsettings
var connectionString = builder.Configuration.GetConnectionString("JobsMarketplace");

// Register the DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Register the Interface for the DBContext
builder.Services.AddScoped<IAppDbContext, AppDbContext>();

// Register services here
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IContractorService, ContractorService>();
builder.Services.AddScoped<IJobOfferService, JobOfferService>();

builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IContractorRepository, ContractorRepository>();
builder.Services.AddScoped<IJobOfferRepository, JobOfferRepository>();

// Caching
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, CacheService>();

// Validator
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<JobValidator>();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Apply migrations first
    context.Database.Migrate();
    // Then seed data
    DbInitializer.Initialize(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    //app.UseSwagger();   
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
