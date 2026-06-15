using MindCalm.Services.Identity.Core.Common.Domain;
using MindCalm.Services.Identity.Core.Common.Enums;
using MindCalm.Services.Identity.Core.Common.Exceptions;
using MindCalm.Services.Identity.Core.Values;

namespace MindCalm.Services.Identity.Core.Entities;

public class User : Entity
{
    public Email? Email { get; private set; }
    public PasswordHash? PasswordHash { get; private set; }
    public UserRole UserRole { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    
    protected User() { }

    private User(Guid id, UserRole userRole)
    {
        Id = id;
        UserRole = userRole;
        CreatedAt = DateTime.UtcNow;
    }

    private User(Guid id, Email email, PasswordHash passwordHash, UserRole userRole)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        UserRole = userRole;
        CreatedAt = DateTime.UtcNow;
    }

    public static User CreateGuest()
    {
        return new User(Guid.CreateVersion7(), UserRole.Guest);
    }

    public static User CreateRegisteredUser(Email email, PasswordHash passwordHash, UserRole userRole = UserRole.Free)
    {
        if (userRole == UserRole.Guest)
            throw new DomainException("Use CreateGuest for guest users.");

        return new User(Guid.CreateVersion7(), email, passwordHash, userRole);
    }

    public void PromoteToRegistered(Email email, PasswordHash passwordHash)
    {
        if (UserRole != UserRole.Guest)
        {
            throw new DomainException("User is already registered.");
        }

        Email = email;
        PasswordHash = passwordHash;
        UserRole = UserRole.Free;
    }

    public void Login()
    {
        UpdatedAt = DateTime.UtcNow;
        LastLoginAt = DateTime.UtcNow;
    }
}