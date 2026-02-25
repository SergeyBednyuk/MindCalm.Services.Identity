using MediatR;
using Microsoft.Extensions.Logging;
using MindCalm.Services.Identity.Core.Common.Models;
using MindCalm.Services.Identity.Core.Interfaces;

namespace MindCalm.Services.Identity.Core.Features.Auth.Login.UserLogin;

public class UserLoginCommandHandler(
    ILogger<UserLoginCommandHandler> logger,
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository
) : IRequestHandler<UserLoginCommand, Result<AuthResult>>
{
    private readonly ILogger<UserLoginCommandHandler> _logger = logger;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<AuthResult>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Email} User logging...", request.Email);

        try
        {
            // 1. GET USER THAT SHOULD BE LOGGED
            var userToLogin = await _userRepository.GetByEmail(request.Email, cancellationToken);
            
            // 2. GUARD CLAUSE: Check if user exists and if password is valid
            if (userToLogin is null)
            {
                _logger.LogWarning("The {Email} user was not found in the database", request.Email);
                return Result<AuthResult>.Failed(data: null, message: "Invalid email or password.");
            }

            if (string.IsNullOrEmpty(userToLogin.PasswordHash) ||
                !_passwordHasher.VerifyPassword(request.Password, userToLogin.PasswordHash))
            {
                _logger.LogWarning("The {Email} user password doesn't match in the database", request.Email);
                return Result<AuthResult>.Failed(data: null, "Invalid email or password.");
            }
            
            // 3. LOGIN AND SAVE TO DATABASE
            userToLogin.Login();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            // 4. GENERATE TOKEN & RETURN
            var token = _jwtTokenGenerator.GenerateToken(userToLogin);
            var result = new AuthResult(userToLogin.Id, token, userToLogin.UserRole.ToString());
            
            return Result<AuthResult>.Success(data: result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging user {Email}", request.Email);
            return Result<AuthResult>.Failed(message: $"The user could not be logged: {ex.Message}");
        }
    }
}