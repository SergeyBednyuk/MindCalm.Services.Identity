using MediatR;
using Microsoft.Extensions.Logging;
using MindCalm.Services.Identity.Core.Common.Models;
using MindCalm.Services.Identity.Core.Entities;
using MindCalm.Services.Identity.Core.Interfaces;

namespace MindCalm.Services.Identity.Core.Features.Auth.Login;

public class GuestLoginCommandHandler(
    ILogger<GuestLoginCommandHandler> logger,
    IJwtTokenGenerator jwtTokenGenerator,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository)
    : IRequestHandler<GuestLoginCommand, Result<AuthResult>>
{
    private readonly ILogger<GuestLoginCommandHandler> _logger = logger;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<Result<AuthResult>> Handle(GuestLoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new guest user...");
        
        try
        {
            var guestUser = User.CreateGuest();

            await _userRepository.AddAsync(guestUser, cancellationToken);

            var affectedRecords = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (affectedRecords == 1)
            {
                var token = _jwtTokenGenerator.GenerateToken(guestUser);
                
                var authResult = new AuthResult(guestUser.Id, token, guestUser.UserRole.ToString());
                
                _logger.LogInformation("the new guest user was created");
                
                return Result<AuthResult>.Success(authResult);
            }
        }
        catch (Exception ex)
        {
            return Result<AuthResult>.Failed(message: "An error occurred while creating the guest user.");
        }
        
        return Result<AuthResult>.Failed(message: "the new guest user was not created.");
    }
}