using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.Infrastructure.Authorization;

namespace TaSked.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AccessPolicies.Member)]
public class HomeworksController : ControllerBase
{
	private readonly IMediator _mediator;

	public HomeworksController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new GetAllHomeworkQuery(userId));
		return Ok(result);
	}
	
	[HttpPost]
	[Authorize(AccessPolicies.Moderator)]
	public async Task<IActionResult> Post(CreateHomeworkRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new CreateHomeworkCommand(
			userId, request.SubjectId, request.Title, request.Description, request.Deadline, request.RelatedLinks));
		return CreatedAtAction(nameof(Get), new { }, result);
	}

    [HttpDelete]
    [Authorize(AccessPolicies.Moderator)]
    public async Task<IActionResult> Delete(DeleteHomeworkRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
		await _mediator.Send(new DeleteHomeworkCommand(userId, request.SubjectId, request.HomeworkId));
        return NoContent();
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("Deadline")]
    public async Task<IActionResult> Patch(ChangeHomeworkDeadlineRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new ChangeHomeworkDeadlineCommand(userId, request.SubjectId, request.HomeworkId, request.HomeworkDeadline));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("Description")]
    public async Task<IActionResult> Patch(ChangeHomeworkDescriptionRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new ChangeHomeworkDescriptionCommand(userId, request.SubjectId, request.HomeworkId, request.HomeworkDescription));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("RelatedLinks")]
    public async Task<IActionResult> Patch(ChangeHomeworkSourceUrlRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(
	        new ChangeHomeworkRelatedLinksCommand(userId, request.SubjectId, request.HomeworkId, request.RelatedLinks));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("Title")]
    public async Task<IActionResult> Patch(ChangeHomeworkTitleRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new ChangeHomeworkTitleCommand(userId, request.SubjectId, request.HomeworkId, request.HomeworkTitle));
        return Ok(result);
    }



    
}