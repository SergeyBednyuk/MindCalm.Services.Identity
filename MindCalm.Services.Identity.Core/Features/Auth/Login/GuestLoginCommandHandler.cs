using MediatR;

namespace MindCalm.Services.Identity.Core.Features.Auth.Login;

public class GuestLoginCommandHandler : IRequestHandler<GuestLoginCommand>
{
    public Task Handle(GuestLoginCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}