using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.Infrastructure.Authorization;

namespace TaSked.Api.Controllers;

[ApiController]
[Route("Subjects/{subjectId:guid}/[controller]")]
[Authorize(AccessPolicies.Member)]
public class HomeworkController : ControllerBase
{
	private readonly IMediator _mediator;

	public HomeworkController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	[Route("~/[controller]")]
	public async Task<IActionResult> Get()
	{
		Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new GetAllHomeworkQuery(userId));
		return Ok(result);
	}
	
	[HttpPost]
	[Authorize(AccessPolicies.Moderator)]
	public async Task<IActionResult> Post(CreateHomeworkRequest request, Guid subjectId)
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new CreateHomeworkCommand(
			userId, subjectId, request.Title, request.Description, request.Deadline, request.RelatedLinks));
		return CreatedAtAction(nameof(Get), new { subjectId = result.SubjectId }, result);
	}

    [HttpDelete]
    [Route("{homeworkId:guid}")]
    [Authorize(AccessPolicies.Moderator)]
    public async Task<IActionResult> Delete(Guid subjectId, Guid homeworkId)
    {
        Guid userId = User.GetUserId()!.Value;
        await _mediator.Send(new DeleteHomeworkCommand(userId, subjectId, homeworkId));
        return NoContent();
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("{homeworkId:guid}/Deadline")]
    public async Task<IActionResult> Patch(ChangeHomeworkDeadlineRequest request, Guid subjectId, Guid homeworkId)
    {
        Guid userId = User.GetUserId()!.Value;
        var result =
	        await _mediator.Send(
		        new ChangeHomeworkDeadlineCommand(userId, subjectId, homeworkId, request.HomeworkDeadline));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("{homeworkId:guid}/Description")]
    public async Task<IActionResult> Patch(ChangeHomeworkDescriptionRequest request, Guid subjectId, Guid homeworkId)
    {
        Guid userId = User.GetUserId()!.Value;
        var result =
	        await _mediator.Send(
		        new ChangeHomeworkDescriptionCommand(userId, subjectId, homeworkId, request.HomeworkDescription));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("{homeworkId:guid}/RelatedLinks")]
    public async Task<IActionResult> Patch(ChangeHomeworkRelatedLinksRequest request, Guid subjectId, Guid homeworkId)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(
	        new ChangeHomeworkRelatedLinksCommand(userId, subjectId, homeworkId, request.RelatedLinks));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("{homeworkId:guid}/Title")]
    public async Task<IActionResult> Patch(ChangeHomeworkTitleRequest request, Guid subjectId, Guid homeworkId)
    {
        Guid userId = User.GetUserId()!.Value;
        var result =
	        await _mediator.Send(new ChangeHomeworkTitleCommand(userId, subjectId, homeworkId, request.HomeworkTitle));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("{homeworkId:guid}/BriefSummary")]
    public async Task<IActionResult> Patch(ChangeHomeworkBriefSummaryRequest request, Guid subjectId, Guid homeworkId)
    {
	    var userId = User.GetUserId()!.Value;
	    var result = await _mediator.Send(
		    new ChangeHomeworkBriefSummaryCommand(userId, subjectId, homeworkId, request.BriefSummary));
	    return Ok(result);
    }

    [HttpGet]
    [Route("{homeworkId:guid}/Comments")]
    public async Task<IActionResult> Get(Guid subjectId, Guid homeworkId)
    {
	    var userId = User.GetUserId()!.Value;
	    var result = await _mediator.Send(new GetHomeworkCommentsQuery(userId, subjectId, homeworkId));
	    return Ok(result);
    }

    [HttpPost]
    [Route("{homeworkId:guid}/Comments")]
    public async Task<IActionResult> Post(CommentHomeworkRequest request, Guid subjectId, Guid homeworkId)
    {
	    var userId = User.GetUserId()!.Value;
	    var result = await _mediator.Send(
		    new CommentHomeworkCommand(userId, subjectId, homeworkId, request.Content));
	    return CreatedAtAction(nameof(Get), new { subjectId, homeworkId }, CommentDTO.From(result));
    }
}
