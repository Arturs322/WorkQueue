using WorkQueue.Application.DTO.Auth;
using WorkQueue.Application.DTO.Dashboard;
using WorkQueue.Application.DTO.WorkItem;
namespace WorkQueue.Application.Interfaces.WorkItems
{
    public interface IWorkItemService
    {
        Task<IEnumerable<WorkItemDto>> GetWorkItemsAsync(WorkItemQueryParameters queryParams, CurrentUserClaims claims);
        Task<WorkItemDto?> GetWorkItemAsync(Guid id, CurrentUserClaims claims);
        Task<WorkItemDto> CreateWorkItemAsync(CreateWorkItemRequest request, CurrentUserClaims currentUserClaims);
        Task UpdateWorkItemAsync(Guid id, UpdateWorkItemRequest request, CurrentUserClaims claims);
        Task AssignWorkItemAsync(Guid id, AssignWorkItemRequest request, CurrentUserClaims claims);
        Task TransitionWorkItemAsync(Guid id, TransitionWorkItemRequest request, CurrentUserClaims claims);
        Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid id, CurrentUserClaims claims);
        Task<CommentDto> AddCommentAsync(Guid id, CreateCommentRequest request, CurrentUserClaims claims);
    }
}