using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindCalm.Services.Identity.Core.Entities;
using MindCalm.Services.Identity.Core.Values;

namespace MindCalm.Services.Identity.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        // Base entity mapping
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("UpdatedAt")
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);

        builder.Property<byte[]>(u => u.RowVersion)
            .IsConcurrencyToken()
            .HasColumnName("RowVersion")
            .IsRowVersion()
            .HasDefaultValueSql("E'\\x01'"); 

        // User entity mapping
        builder.Property(u => u.Email)
            .HasColumnName("Email")
            .HasMaxLength(255)
            .HasConversion(
                v => v != null ? v.Value : null, // Convert Email object to string for DB
                v => v != null ? Email.Create(v) : null // Convert string back to Email object
            );

        builder.Property(u => u.PasswordHash)
            .HasColumnName("PasswordHash")
            .HasMaxLength(255)
            .HasConversion(
                p => p != null ? p.Value : null,
                p => p != null ? PasswordHash.Create(p) : null
            );

        builder.Property(u => u.UserRole)
            .HasConversion<int>() 
            .HasColumnName("UserRole")
            .HasColumnType("integer")
            .IsRequired();
        
        builder.Property(u => u.LastLoginAt)
            .IsRequired(false);
    }
}