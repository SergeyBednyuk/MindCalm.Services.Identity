using MediatR;
using Microsoft.AspNetCore.Mvc;
using MindCalm.Services.Identity.Core.Features.Auth.Login;
using MindCalm.Services.Identity.Core.Features.Auth.Register;

namespace MindCalm.Services.Identity.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("guest")]
    public async Task<IActionResult> LoginGuest(CancellationToken cancellationToken)
    {
        var command = new GuestLoginCommand();
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(new { error = result.Message });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        
        return BadRequest(new { error = result.Message });
    }
}