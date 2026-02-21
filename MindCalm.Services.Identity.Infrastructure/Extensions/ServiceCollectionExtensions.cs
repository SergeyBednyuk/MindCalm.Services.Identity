using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MindCalm.Services.Identity.Core.Interfaces;
using MindCalm.Services.Identity.Infrastructure.Data;
using MindCalm.Services.Identity.Infrastructure.Repositories;
using MindCalm.Services.Identity.Infrastructure.Services;
using Npgsql;

namespace MindCalm.Services.Identity.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Build the Connection String dynamically
        var dbBuilder = new NpgsqlConnectionStringBuilder()
        {
            Host = configuration["DB_HOST"] ?? "localhost",
            Port = 5433,
            Database = configuration["DB_NAME"] ?? "mindcalm_identity", // Postgres prefers lowercase
            Username = configuration["DB_USER"] ?? "postgres",
            Password = configuration["DB_PASSWORD"] ?? "Legion13"
        };
        
        
        // 2. Register DbContext
        services.AddDbContext<MindCalmIdentityDbContext>(options =>
            options.UseNpgsql(dbBuilder.ConnectionString));

        // 3. Register Repositories and Unit of Work
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        
        return services;
    }
}