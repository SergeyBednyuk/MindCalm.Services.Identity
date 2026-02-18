namespace MindCalm.Services.Identity.Core.Common.Exceptions;

public class UserAlreadyExistsException(string email) : DomainException($"User with email {email} already exist") { }