using WorkQueue.Domain.Enums;

namespace WorkQueue.Application.DTO.WorkItem
{
    public class CreateWorkItemRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public WorkItemPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }
}