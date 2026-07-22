using WorkQueue.Domain.Enums;

namespace WorkQueue.Application.DTO.WorkItem
{
    public class UpdateWorkItemRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public WorkItemPriority? Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
