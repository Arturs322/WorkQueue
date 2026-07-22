using Microsoft.AspNetCore.Mvc;
using WorkQueue.Application.Interfaces.Dashboard;

namespace WorkQueue.API.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController(IDashboardService _dashboardService) : ControllerBase
    {
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _dashboardService.GetSummaryAsync();

            return Ok(result);
        }
    }
}
