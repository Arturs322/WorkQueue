using Microsoft.AspNetCore.Mvc;
using WorkQueue.Application.DTO.Auth;
using WorkQueue.Application.Interfaces.Authentication;

namespace WorkQueue.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);

            return Ok(response);
        }
    }
}