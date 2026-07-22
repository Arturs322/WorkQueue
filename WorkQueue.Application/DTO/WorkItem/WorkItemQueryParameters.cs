using WorkQueue.Domain.Enums;

namespace WorkQueue.Application.DTO.WorkItem
{
    public class WorkItemQueryParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public WorkItemStatus? Status { get; set; }
        public WorkItemPriority? Priority { get; set; }
        public Guid? AssigneeUserId { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;
    }
}
