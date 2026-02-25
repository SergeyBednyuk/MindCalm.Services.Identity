using MediatR;
using MindCalm.Services.Identity.Core.Common.Models;

namespace MindCalm.Services.Identity.Core.Features.Auth.Login.UserLogin;

public record UserLoginCommand(string Email, string Password) : IRequest<Result<AuthResult>> { }