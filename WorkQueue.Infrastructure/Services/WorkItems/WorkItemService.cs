using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkQueue.Application.DTO.Auth;
using WorkQueue.Application.DTO.WorkItem;
using WorkQueue.Application.Interfaces.Users;
using WorkQueue.Application.Interfaces.WorkItems;
using WorkQueue.DataAccess;
using WorkQueue.Domain.Entities;
using WorkQueue.Domain.Enums;

namespace WorkQueue.Infrastructure.Services.WorkItems
{
    public class WorkItemService(ApplicationDbContext _db, IMapper _mapper, IUserService _userService) : IWorkItemService
    {
        private static readonly Dictionary<WorkItemStatus, List<WorkItemStatus>> AllowedTransitions = new()
        {
            [WorkItemStatus.New] = [WorkItemStatus.InProgress, WorkItemStatus.Blocked],
            [WorkItemStatus.InProgress] = [WorkItemStatus.Blocked, WorkItemStatus.Done],
            [WorkItemStatus.Blocked] = [WorkItemStatus.InProgress, WorkItemStatus.Done],
            [WorkItemStatus.Done] = [WorkItemStatus.InProgress]
        };

        public async Task<IEnumerable<WorkItemDto>> GetWorkItemsAsync(WorkItemQueryParameters queryParams, CurrentUserClaims claims)
        {
            var query = _db.WorkItems.AsNoTracking().Where(x => x.OrganizationId == claims.OrganizationId);

            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var search = queryParams.SearchTerm.Trim().ToLower();
                query = query.Where(x => x.Title.ToLower().Contains(search) || (x.Description != null && x.Description.ToLower().Contains(search)));
            }

            if (queryParams.Status.HasValue)
                query = query.Where(x => x.Status == queryParams.Status.Value);

            if (queryParams.Priority.HasValue)
                query = query.Where(x => x.Priority == queryParams.Priority.Value);

            if (queryParams.AssigneeUserId.HasValue)
                query = query.Where(x => x.AssigneeUserId == queryParams.AssigneeUserId.Value);

            query = queryParams.SortBy?.ToLower() switch
            {
                "duedate" => queryParams.IsDescending ? query.OrderByDescending(x => x.DueDate) : query.OrderBy(x => x.DueDate),
                "priority" => queryParams.IsDescending ? query.OrderByDescending(x => x.Priority) : query.OrderBy(x => x.Priority),
                "status" => queryParams.IsDescending ? query.OrderByDescending(x => x.Status) : query.OrderBy(x => x.Status),
                _ => queryParams.IsDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt)
            };

            var items = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<WorkItemDto>>(items);
        }

        public async Task<WorkItemDto?> GetWorkItemAsync(Guid id, CurrentUserClaims claims)
        {
            var workItem = await _db.WorkItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == claims.OrganizationId);

            if (workItem == null)
                throw new KeyNotFoundException("Work item was not found.");

            return _mapper.Map<WorkItemDto>(workItem);
        }

        public async Task<WorkItemDto> CreateWorkItemAsync(CreateWorkItemRequest request, CurrentUserClaims currentUserClaims)
        {
            var newWorkItem = new WorkItem
            {
                Title = request.Title,
                OrganizationId = currentUserClaims.OrganizationId,
                Status = WorkItemStatus.New,
                Description = request.Description,
                Priority = request.Priority,
                DueDate = request.DueDate,
                CreatedByUserId = currentUserClaims.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _db.AddAsync(newWorkItem);
            await _db.SaveChangesAsync();

            return _mapper.Map<WorkItemDto>(newWorkItem) ?? throw new InvalidOperationException("Work item was not created.");
        }

        public async Task UpdateWorkItemAsync(Guid id, UpdateWorkItemRequest request, CurrentUserClaims claims)
        {
            var workItem = await _db.WorkItems.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == claims.OrganizationId)
                ?? throw new KeyNotFoundException("Work item was not found.");
            if (workItem.Status == WorkItemStatus.Done && claims.Role == UserRole.Member.ToString())
            {
                throw new InvalidOperationException("Members cannot edit work items in status 'Done'.");
            }
            _mapper.Map(request, workItem);
            workItem.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        public async Task AssignWorkItemAsync(Guid workItemId, AssignWorkItemRequest request, CurrentUserClaims claims)
        {
            if (claims.Role != UserRole.Manager.ToString())
            {
                throw new InvalidOperationException("Only managers can assign work items.");
            }

            var workItem = await _db.WorkItems.FirstOrDefaultAsync(x => x.Id == workItemId && x.OrganizationId == claims.OrganizationId)
                ?? throw new KeyNotFoundException("Work item was not found.");

            var targetUser = await _userService.GetByIdAsync(request.UserId);
            if (targetUser == null || targetUser.OrganizationId != claims.OrganizationId)
            {
                throw new InvalidOperationException("Target user does not exist in your organization.");
            }

            workItem.AssigneeUserId = request.UserId;
            workItem.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        public async Task TransitionWorkItemAsync(Guid id, TransitionWorkItemRequest request, CurrentUserClaims claims)
        {
            var workItem = await _db.WorkItems.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == claims.OrganizationId)
                ?? throw new KeyNotFoundException("Work item was not found.");

            if (workItem.Status == request.Status)
                return;

            if (!AllowedTransitions[workItem.Status].Contains(request.Status))
            {
                throw new ArgumentException(
                    $"Invalid status transition from '{workItem.Status}' to '{request.Status}'."
                );
            }

            if (workItem.Status == WorkItemStatus.Done && request.Status != WorkItemStatus.Done)
            {
                if (claims.Role != UserRole.Manager.ToString())
                {
                    throw new InvalidOperationException("Only a manager can reopen a 'Done' work item.");
                }
            }

            workItem.Status = request.Status;
            workItem.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid id, CurrentUserClaims claims)
        {
            return _mapper.Map<IEnumerable<CommentDto>>(await _db.Comments.Where(x => x.WorkItemId == id && x.OrganizationId == claims.OrganizationId).ToListAsync());
        }

        public async Task<CommentDto> AddCommentAsync(Guid id, CreateCommentRequest request, CurrentUserClaims claims)
        {
            var workItem = await _db.WorkItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == claims.OrganizationId)
                ?? throw new KeyNotFoundException("Work item was not found.");

            if (workItem.OrganizationId != claims.OrganizationId)
            {
                throw new InvalidOperationException("Wrong Organization.");
            }

            var newComment = new Comment
            {
                WorkItemId = id,
                OrganizationId = claims.OrganizationId,
                UserId = claims.UserId,
                Text = request.Text
            };
            await _db.Comments.AddAsync(newComment);
            await _db.SaveChangesAsync();

            return _mapper.Map<CommentDto>(newComment) ?? throw new InvalidOperationException("Comment was not created.");

        }
    }
}
