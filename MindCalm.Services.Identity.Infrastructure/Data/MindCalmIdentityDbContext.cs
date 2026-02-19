using Microsoft.EntityFrameworkCore;
using MindCalm.Services.Identity.Core.Entities;

namespace MindCalm.Services.Identity.Infrastructure.Data;

public class MindCalmIdentityDbContext(DbContextOptions<MindCalmIdentityDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasIndex(e => e.Email)
                  .IsUnique()
                  .HasFilter("[Email] IS NOT NULL");

            entity.Property(e => e.UserRole)
                  .HasConversion<string>();

            entity.Property(e => e.RowVersion)
                  .IsRowVersion();
        });
    }
}