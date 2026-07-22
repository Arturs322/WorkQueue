namespace WorkQueue.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid WorkItemId { get; set; }
        public WorkItem WorkItem { get; set; } = null!;
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;
        public Guid UserId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
