using MindCalm.Services.Identity.Core.Common.Exceptions;

namespace MindCalm.Services.Identity.Core.Values;

public sealed class PasswordHash
{
    public string Value { get; }

    private PasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("PasswordHash cannot be empty.");

        Value = value;
    }

    public static PasswordHash CreateHash(string passwordText)
    {
        var hashed = BCrypt.Net.BCrypt.EnhancedHashPassword(passwordText, 13);
        return new PasswordHash(hashed);
    }

    public bool Verify(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Value);
    }

    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        return obj is PasswordHash otherPasswordHashValue && Value == otherPasswordHashValue.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}