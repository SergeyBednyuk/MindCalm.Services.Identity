namespace MindCalm.Services.Identity.Core.Features.Auth.Login;

public record AuthResult(Guid UserId, string Token, string Role) { }