using MediatR;
using MindCalm.Services.Identity.Core.Common.Models;

namespace MindCalm.Services.Identity.Core.Features.Auth.Register;

public record RegisterCommand(Guid? UserId, string Email, string Password) : IRequest<Result<RegisterResult>> { }