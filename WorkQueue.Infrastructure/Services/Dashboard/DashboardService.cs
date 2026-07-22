using Microsoft.EntityFrameworkCore;
using WorkQueue.Application.DTO.Auth;
using WorkQueue.Application.DTO.Dashboard;
using WorkQueue.Application.Interfaces.Dashboard;
using WorkQueue.DataAccess;
using WorkQueue.Domain.Enums;

namespace WorkQueue.Infrastructure.Services.Dashboard
{
    public class DashboardService(ApplicationDbContext _db) : IDashboardService
    {
        public async Task<DashboardSummaryDto> GetSummaryAsync(CurrentUserClaims claims)
        {
            var query = _db.WorkItems.AsNoTracking().Where(x => x.OrganizationId == claims.OrganizationId);
            return new DashboardSummaryDto
            {
                TotalWorkItems = await query.CountAsync(),
                NewWorkItems = await query.CountAsync(x => x.Status == WorkItemStatus.New),
                InProgressWorkItems = await query.CountAsync(x => x.Status == WorkItemStatus.InProgress),
                BlockedWorkItems = await query.CountAsync(x => x.Status == WorkItemStatus.Blocked),
                DoneWorkItems = await query.CountAsync(x => x.Status == WorkItemStatus.Done),
                AssignedToMe = await query.CountAsync(x => x.AssigneeUserId == claims.UserId),
                OverdueWorkItems = await query.CountAsync(x => x.DueDate < DateTime.UtcNow && x.Status != WorkItemStatus.Done)
            };
        }
    }
}
