using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace MindCalm.Services.Identity.Infrastructure.Data;

public class MindCalmIdentityDbContextFactory : IDesignTimeDbContextFactory<MindCalmIdentityDbContext>
{
    public MindCalmIdentityDbContext CreateDbContext(string[] args)
    {
        var connectionString = new NpgsqlConnectionStringBuilder()
        {
            Host = "localhost", // Or Environment.GetEnvironmentVariable("DB_HOST")
            Port = 5433,        // Or Environment.GetEnvironmentVariable("DB_PORT")
            Database = "mindcalm_identity", // Or Environment.GetEnvironmentVariable("DB_NAME")
            Username = "postgres", // Or Environment.GetEnvironmentVariable("DB_USER")
            Password = "Legion13"  // Or Environment.GetEnvironmentVariable("DB_PASSWORD")
        }.ConnectionString;

        var optionsBuilder = new DbContextOptionsBuilder<MindCalmIdentityDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new MindCalmIdentityDbContext(optionsBuilder.Options);
    }
}