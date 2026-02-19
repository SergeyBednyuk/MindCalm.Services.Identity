using MediatR;
using MindCalm.Services.Identity.Core.Common.Models;

namespace MindCalm.Services.Identity.Core.Features.Auth.Login;

public class GuestLoginCommand : IRequest<Result<AuthResult>>
{
    
}