namespace MindCalm.Services.Identity.Core.Features.Auth.Register;

// Don't like implementation. TODO???
public record RegisterResult(Guid UserId, string Token, string Role) { }