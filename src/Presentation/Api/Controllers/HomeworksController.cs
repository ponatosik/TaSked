using TaSked.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaSked.Application;
using Microsoft.AspNetCore.Authorization;
using TaSked.Infrastructure.Authorization;
using TaSked.Api.Requests;

namespace TaSked.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AccessPolicise.Member)]
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
	[Authorize(AccessPolicise.Moderator)]
	public async Task<IActionResult> Post(CreateHomeworkRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new CreateHomeworkCommand(userId, request.SubjectId, request.Title, request.Description));
		return CreatedAtAction(nameof(Get), new { }, result);
	}

    [HttpDelete]
    [Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Delete(DeleteHomeworkRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
		await _mediator.Send(new DeleteHomeworkCommand(userId, request.SubjectId, request.HomeworkId));
        return NoContent();
    }

    [HttpPatch]
    [Authorize(AccessPolicise.Moderator)]
    [Route("Deadline")]
    public async Task<IActionResult> Patch(ChangeHomeworkDeadlineRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new ChangeHomeworkDeadlineCommand(userId, request.SubjectId, request.HomeworkId, request.HomeworkDeadline));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(AccessPolicise.Moderator)]
    [Route("Description")]
    public async Task<IActionResult> Patch(ChangeHomeworkDescriptionRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new ChangeHomeworkDescriptionCommand(userId, request.SubjectId, request.HomeworkId, request.HomeworkDescription));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(AccessPolicise.Moderator)]
    [Route("SourceUrl")]
    public async Task<IActionResult> Patch(ChangeHomeworkSourceUrlRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new ChangeHomeworkSourceUrlCommand(userId, request.SubjectId, request.HomeworkId, request.HomeworkSourceUrl));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(AccessPolicise.Moderator)]
    [Route("Title")]
    public async Task<IActionResult> Patch(ChangeHomeworkTitleRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new ChangeHomeworkTitleCommand(userId, request.SubjectId, request.HomeworkId, request.HomeworkTitle));
        return Ok(result);
    }
}