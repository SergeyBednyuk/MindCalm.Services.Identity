using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindCalm.Services.Identity.Core.Features.Auth.Login.GuestLogin;
using MindCalm.Services.Identity.Core.Features.Auth.Login.UserLogin;
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

        return BadRequest(new { error = result.Message, messages = result.Errors });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        
        return BadRequest(new { error = result.Message, messages = result.Errors });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        
        return BadRequest(new { error = result.Message, messages = result.Errors });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult MeTest(CancellationToken cancellationToken)
    {
        // Because of [Authorize], this code only runs if the token is 100% valid!
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new { Message = "You are authenticated!", UserId = userId, Role = role });
    }
}