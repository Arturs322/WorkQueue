using WorkQueue.Application.DTO.Auth;

namespace WorkQueue.Application.Interfaces.Authentication
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    }
}
