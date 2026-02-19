using MindCalm.Services.Identity.Core.Common.Enums;
using MindCalm.Services.Identity.Core.Common.Exceptions;

namespace MindCalm.Services.Identity.Core.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string? Email { get; private set; }
    public string? PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public UserRole UserRole { get; private set; }
    public byte[] RowVersion { get; private set; }
    
    protected User() { }

    private User(Guid id, UserRole userRole)
    {
        Id = id;
        UserRole = userRole;
        CreatedAt = DateTime.UtcNow;
    }

    public static User CreateGuest()
    {
        return new User(Guid.CreateVersion7(), UserRole.Guest);
    }

    public static User CreateRegisteredUser(string email, string passwordHash, UserRole userRole = UserRole.Free)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email cannot be empty.");
        }

        return new User(Guid.CreateVersion7(), userRole)
        {
            Email = email,
            PasswordHash = passwordHash
        };
    }

    public void PromoteToRegistered(string email, string passwordHash)
    {
        if (UserRole != UserRole.Guest)
        {
            throw new DomainException("User is already registered.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email is required for registration.");
        }
        
        Email = email;
        PasswordHash = passwordHash;
        UserRole = UserRole.Free;
    }

    public void Login()
    {
        LastLoginAt = DateTime.UtcNow;
    }
}