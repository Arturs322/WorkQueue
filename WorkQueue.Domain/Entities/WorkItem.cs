using WorkQueue.Domain.Enums;

namespace WorkQueue.Domain.Entities
{
    public class WorkItem
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public WorkItemStatus Status { get; set; }
        public WorkItemPriority Priority { get; set; }
        public Guid? AssigneeUserId { get; set; }
        public User? AssigneeUser { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public uint Version { get; set; }
    }
}
