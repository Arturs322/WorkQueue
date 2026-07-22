using System.Security.Claims;
using WorkQueue.Application.DTO.Auth;
using WorkQueue.Domain.Entities;

namespace WorkQueue.Application.Interfaces.Authentication
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        CurrentUserClaims GetCurrentUserClaims(ClaimsPrincipal user);
    }
}
