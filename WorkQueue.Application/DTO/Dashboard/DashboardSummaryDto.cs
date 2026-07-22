namespace WorkQueue.Application.DTO.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalWorkItems { get; set; }

        public int NewWorkItems { get; set; }

        public int InProgressWorkItems { get; set; }

        public int BlockedWorkItems { get; set; }

        public int DoneWorkItems { get; set; }

        public int AssignedToMe { get; set; }

        public int OverdueWorkItems { get; set; }
    }
}
