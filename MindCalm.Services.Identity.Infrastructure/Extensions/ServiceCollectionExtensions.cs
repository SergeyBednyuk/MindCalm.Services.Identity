using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MindCalm.Services.Identity.Core.Interfaces;
using MindCalm.Services.Identity.Infrastructure.Data;
using MindCalm.Services.Identity.Infrastructure.Repositories;
using MindCalm.Services.Identity.Infrastructure.Services;

namespace MindCalm.Services.Identity.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Build the Connection String dynamically
        var dbBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = configuration["DB_HOST"] ?? @"(localdb)\\mssqllocaldb",
            InitialCatalog = configuration["DB_NAME"] ?? "MindCalm_Identity",
            UserID = configuration["DB_USER"] ?? "",
            Password = configuration["DB_PASSWORD"] ?? "",
            
            Encrypt = true,
            TrustServerCertificate = true, // Set to false in real production
            MultipleActiveResultSets = true
        };

        if (string.IsNullOrEmpty(dbBuilder.UserID))
        {
            dbBuilder.IntegratedSecurity = true;
        }
        
        // 2. Register DbContext
        services.AddDbContext<MindCalmIdentityDbContext>(options =>
            options.UseSqlServer(dbBuilder.ConnectionString));

        // 3. Register Repositories and Unit of Work
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        
        return services;
    }
}