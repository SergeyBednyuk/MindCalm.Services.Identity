using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MindCalm.Services.Identity.Core.Entities;
using MindCalm.Services.Identity.Core.Interfaces;

namespace MindCalm.Services.Identity.Infrastructure.Services;

public class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration = configuration;
    public string GenerateToken(User user)
    {
        // 1. Get Secret Key (Fallback for local dev)
        var secretKey = _configuration["JwtSettings:Secret"] ?? "MindCalm-Super-Secret-Key-Must-Be-At-Least-32-Chars!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 2. Define the Claims (The data inside the token)
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.UserRole.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // 3. Create the Token
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"] ?? "MindCalm.Identity",
            audience: _configuration["JwtSettings:Audience"] ?? "MindCalm.App",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7), // Guests stay logged in for a week
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}