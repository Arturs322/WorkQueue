namespace WorkQueue.Application.DTO.WorkItem
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid WorkItemId { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
