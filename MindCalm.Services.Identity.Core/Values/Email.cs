using MindCalm.Services.Identity.Core.Common.Exceptions;

namespace MindCalm.Services.Identity.Core.Values;

public sealed class Email
{
    public string Value { get; }

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email cannot be empty.");

        if (!value.Contains("@"))
            throw new DomainException("Invalid email format.");

        Value = value.Trim().ToLowerInvariant();
    }

    public static Email Create(string email)
    {
        return new Email(email);
    }

    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        return obj is Email otherEmailValue && Value == otherEmailValue.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}