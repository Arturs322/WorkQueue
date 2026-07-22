using System;
using System.Collections.Generic;
using System.Text;
using WorkQueue.Application.DTO.Dashboard;
using WorkQueue.Application.Interfaces.Dashboard;

namespace WorkQueue.Infrastructure.Services.Dashboard
{
    public class DashboardService() : IDashboardService
    {
        public Task<DashboardSummaryDto> GetSummaryAsync()
        {
            throw new NotImplementedException();
        }
    }
}
