using MediatR;
using MindCalm.Services.Identity.Core.Common.Models;

namespace MindCalm.Services.Identity.Core.Features.Auth.Login;

public class GuestLoginCommandHandler : IRequestHandler<GuestLoginCommand, Result<AuthResult>>
{
    public Task<Result<AuthResult>> Handle(GuestLoginCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}