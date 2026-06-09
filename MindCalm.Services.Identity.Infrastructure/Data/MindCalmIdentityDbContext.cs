using Microsoft.EntityFrameworkCore;
using MindCalm.Services.Identity.Core.Entities;
using MindCalm.Services.Identity.Infrastructure.Data.Configurations;

namespace MindCalm.Services.Identity.Infrastructure.Data;

public class MindCalmIdentityDbContext : DbContext
{
    public MindCalmIdentityDbContext() : base()
    {
    }

    public MindCalmIdentityDbContext(DbContextOptions<MindCalmIdentityDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}