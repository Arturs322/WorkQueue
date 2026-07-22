using WorkQueue.Domain.Enums;

namespace WorkQueue.Application.DTO.WorkItem
{
    public class WorkItemDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public WorkItemStatus Status { get; set; }
        public WorkItemPriority Priority { get; set; }
        public Guid? AssigneeUserId { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
