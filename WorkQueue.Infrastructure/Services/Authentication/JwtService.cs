using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkQueue.Application.DTO.Auth;
using WorkQueue.Application.Interfaces.Authentication;
using WorkQueue.Domain.Entities;

namespace WorkQueue.Infrastructure.Services.Authentication
{
    public class JwtService(IConfiguration _configuration) : IJwtService
    {
        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.Id.ToString()),
                new Claim(
                    "organizationId",
                    user.OrganizationId.ToString()),
                new Claim(
                    ClaimTypes.Role,
                    user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["Jwt:ExpirationMinutes"]!)
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public CurrentUserClaims GetCurrentUserClaims(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            var orgIdClaim = user.FindFirst("organizationId")?.Value;

            var roleClaim = user.FindFirst(ClaimTypes.Role)?.Value;

            return new CurrentUserClaims
            {
                UserId = userIdClaim != null ? Guid.Parse(userIdClaim) : Guid.Empty,
                OrganizationId = orgIdClaim != null ? Guid.Parse(orgIdClaim) : Guid.Empty,
                Role = roleClaim ?? string.Empty
            };
        }
    }
}
