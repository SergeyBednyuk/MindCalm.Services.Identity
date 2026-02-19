using MindCalm.Services.Identity.Core.Entities;

namespace MindCalm.Services.Identity.Core.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}