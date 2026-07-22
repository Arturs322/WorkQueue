using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkQueue.Application.DTO.Auth;
using WorkQueue.Application.DTO.WorkItem;
using WorkQueue.Application.Interfaces.Authentication;
using WorkQueue.Application.Interfaces.WorkItems;

namespace WorkQueue.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/work-items")]
    public class WorkItemsController(IWorkItemService _workItemService, IJwtService _jwtService) : ControllerBase
    {
        protected CurrentUserClaims CurrentUser => _jwtService.GetCurrentUserClaims(User);

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] WorkItemQueryParameters queryParams)
        {
            var result = await _workItemService.GetWorkItemsAsync(queryParams, CurrentUser);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _workItemService.GetWorkItemAsync(id, CurrentUser);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWorkItemRequest request)
        {
            var result = await _workItemService.CreateWorkItemAsync(request, CurrentUser);
            return Ok(result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateWorkItemRequest request)
        {
            await _workItemService.UpdateWorkItemAsync(id, request, CurrentUser);
            return NoContent();
        }

        [HttpPost("{id:guid}/assign")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Assign(Guid id, AssignWorkItemRequest request)
        {
            await _workItemService.AssignWorkItemAsync(id, request, CurrentUser);
            return NoContent();
        }

        [HttpPost("{id:guid}/transition")]
        public async Task<IActionResult> Transition(Guid id, TransitionWorkItemRequest request)
        {
            await _workItemService.TransitionWorkItemAsync(id, request, CurrentUser);
            return NoContent();
        }

        [HttpGet("{id:guid}/comments")]
        public async Task<IActionResult> GetComments(Guid id)
        {
            var result = await _workItemService.GetCommentsAsync(id, CurrentUser);
            return Ok(result);
        }

        [HttpPost("{id:guid}/comments")]
        public async Task<IActionResult> AddComment(Guid id, CreateCommentRequest request)
        {
            var result = await _workItemService.AddCommentAsync(id, request, CurrentUser);
            return Ok(result);
        }
    }
}