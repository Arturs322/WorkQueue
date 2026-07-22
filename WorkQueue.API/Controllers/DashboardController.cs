using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkQueue.Application.Interfaces.Authentication;
using WorkQueue.Application.Interfaces.Dashboard;

namespace WorkQueue.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/dashboard")]
    public class DashboardController(IDashboardService _dashboardService, IJwtService _jwtService) : ControllerBase
    {
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var claims = _jwtService.GetCurrentUserClaims(User);
            var result = await _dashboardService.GetSummaryAsync(claims);

            return Ok(result);
        }
    }
}
