using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] WorkItemQueryParameters queryParams)
        {
            var claims = _jwtService.GetCurrentUserClaims(User);
            var result = await _workItemService.GetWorkItemsAsync(queryParams, claims);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var claims = _jwtService.GetCurrentUserClaims(User);
            var result = await _workItemService.GetWorkItemAsync(id, claims);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWorkItemRequest request)
        {
            var claims = _jwtService.GetCurrentUserClaims(User);
            var result = await _workItemService.CreateWorkItemAsync(request, claims);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateWorkItemRequest request)
        {
            var claims = _jwtService.GetCurrentUserClaims(User);
            await _workItemService.UpdateWorkItemAsync(id, request, claims);

            return NoContent();
        }

        [HttpPost("{id:guid}/assign")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Assign(Guid id, AssignWorkItemRequest request)
        {
            var claims = _jwtService.GetCurrentUserClaims(User);
            await _workItemService.AssignWorkItemAsync(id, request, claims);

            return NoContent();
        }

        [HttpPost("{id:guid}/transition")]
        public async Task<IActionResult> Transition(Guid id, TransitionWorkItemRequest request)
        {
            var claims = _jwtService.GetCurrentUserClaims(User);
            await _workItemService.TransitionWorkItemAsync(id, request, claims);

            return NoContent();
        }

        [HttpGet("{id:guid}/comments")]
        public async Task<IActionResult> GetComments(Guid id)
        {
            var result = await _workItemService.GetCommentsAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("{id:guid}/comments")]
        public async Task<IActionResult> AddComment(Guid id, CreateCommentRequest request)
        {
            var result = await _workItemService.AddCommentAsync(id, request);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}