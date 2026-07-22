using WorkQueue.Application.DTO.Dashboard;

namespace WorkQueue.Application.Interfaces.Dashboard
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
    }
}
