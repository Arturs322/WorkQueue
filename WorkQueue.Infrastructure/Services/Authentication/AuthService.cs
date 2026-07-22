using Microsoft.AspNetCore.Identity;
using WorkQueue.Application.DTO.Auth;
using WorkQueue.Application.Interfaces.Authentication;
using WorkQueue.Application.Interfaces.Users;
using WorkQueue.Domain.Entities;

namespace WorkQueue.Infrastructure.Services.Authentication
{
    public class AuthService(IUserService _userService, IJwtService _jwtService, IPasswordHasher<User> _passwordHasher) : IAuthService
    {
        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userService.GetByEmailAsync(loginRequest.Email) ?? throw new UnauthorizedAccessException();

            var result = _passwordHasher
                .VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    loginRequest.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException();

            return new LoginResponse
            {
                Token = _jwtService.GenerateToken(user),
                Profile = new UserProfile
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    OrganizationId = user.OrganizationId
                }
            };
        }
    }
}
