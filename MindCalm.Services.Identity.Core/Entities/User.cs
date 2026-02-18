using MindCalm.Services.Identity.Core.Common.Enums;
using MindCalm.Services.Identity.Core.Common.Exceptions;

namespace MindCalm.Services.Identity.Core.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string? Email { get; private set; }
    public string? PasswordHash { get; private set; }
    public DateTime CreatedAd { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public UserRole UserRole { get; private set; }
    public byte[] RawVervion { get; private set; }
    
    protected User() { }

    private User(Guid id, UserRole userRole)
    {
        Id = id;
        UserRole = userRole;
        CreatedAd = DateTime.UtcNow;
    }

    public static User CreateGuest()
    {
        return new User(Guid.NewGuid(), UserRole.Guest);
    }

    public static User CreateRegisteredUser(string email, string passwordHash, UserRole userRole = UserRole.Free)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email cannot be empty.");
        }

        return new User(Guid.NewGuid(), userRole)
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
    }

    public void Login()
    {
        LastLoginAt = DateTime.UtcNow;
    }
}