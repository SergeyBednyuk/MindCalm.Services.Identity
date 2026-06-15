using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MindCalm.Services.Identity.Core.Common.Models;
using MindCalm.Services.Identity.Core.Entities;
using MindCalm.Services.Identity.Core.Features.Auth.Login;
using MindCalm.Services.Identity.Core.Interfaces;
using MindCalm.Services.Identity.Core.Values;

namespace MindCalm.Services.Identity.Core.Features.Auth.Register;

public class RegisterCommandHandler(
    ILogger<RegisterCommandHandler> logger,
    IJwtTokenGenerator jwtTokenGenerator,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository) 
    : IRequestHandler<RegisterCommand, Result<RegisterResult>>
{
    public async Task<Result<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Registering new user: {Email}", request.Email);

        try
        {
            // 1. GUARD CLAUSE: Check Email Uniqueness early
            if (!await userRepository.IsEmailUniqueAsync(request.Email, cancellationToken))
            {
                return Result<RegisterResult>.Failed(message: $"User with {request.Email} email already exist.");
            }

            var email = Email.Create(request.Email);
            var hashedPassword = PasswordHash.CreateHash(request.Password);
            User userToSave;

            // 2. SCENARIO A: Promote Existing Guest
            if (request.UserId.HasValue)
            {
                var guest = await userRepository.GetByIdAsync(request.UserId.Value, cancellationToken);
                if (guest is null)
                {
                    logger.LogWarning("Attempted to register with invalid Guest ID: {UserId}", request.UserId);
                    return Result<RegisterResult>.Failed(message: $"There is no user with id {request.UserId}");
                }

                guest.PromoteToRegistered(email, hashedPassword);
                userToSave = guest;
            }
            // 3. SCENARIO B: Direct Registration (New User)
            else
            {
                userToSave = User.CreateRegisteredUser(email, hashedPassword);
                await userRepository.AddAsync(userToSave, cancellationToken);
            }

            // 4. COMMIT TO DATABASE
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 5. GENERATE TOKEN & RETURN
            var token = jwtTokenGenerator.GenerateToken(userToSave);
            var result = new RegisterResult(userToSave.Id, token, userToSave.UserRole.ToString());

            logger.LogInformation("User {UserId} successfully registered", userToSave.Id);
            return Result<RegisterResult>.Success(result);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, "Error registering user {Email}", request.Email);
            return Result<RegisterResult>.Failed(message: $"The user could not be registered: {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error registering user {Email}", request.Email);
            return Result<RegisterResult>.Failed(message: $"The user could not be registered: {ex.Message}");
        }
    }
}