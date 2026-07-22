using WorkQueue.Domain.Enums;

namespace WorkQueue.Application.DTO.WorkItem
{
    public class TransitionWorkItemRequest
    {
        public WorkItemStatus Status { get; set; }
    }
}
