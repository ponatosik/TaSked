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
public class LessonsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LessonsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(AccessPolicies.Moderator)]
    public async Task<IActionResult> Post(CreateLessonRequest request, Guid subjectId)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(
	        new CreateLessonCommand(userId, subjectId, request.LessonTime, request.LessonLink));
        return CreatedAtAction(nameof(Get), new { subjectId = result.SubjectId }, result);
    }

    [HttpDelete]
    [Route("{lessonId:guid}")]
    [Authorize(AccessPolicies.Moderator)]
    public async Task<IActionResult> Delete(Guid subjectId, Guid lessonId)
    {
        Guid userId = User.GetUserId()!.Value;
        await _mediator.Send(new DeleteLessonCommand(userId, subjectId, lessonId));
        return NoContent();
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("{lessonId:guid}/Time")]
    public async Task<IActionResult> Patch(ChangeLessonTimeRequest request, Guid subjectId, Guid lessonId)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new ChangeLessonTimeCommand(userId, subjectId, lessonId, request.NewTime));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("{lessonId:guid}/Link")]
    public async Task<IActionResult> Patch(ChangeLessonLinkRequest request, Guid subjectId, Guid lessonId)
    {
	    var userId = User.GetUserId()!.Value;
	    var result = await _mediator.Send(
		    new ChangeLessonLinkCommand(userId, subjectId, lessonId, request.NewLink));
	    return Ok(result);
    }


    [HttpGet]
    public async Task<IActionResult> Get(Guid subjectId)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new GetAllLessonsBySubjectQuery(userId, subjectId));
        return Ok(result);
    }

    [HttpGet]
    [Route("~/[controller]")]
    public async Task<IActionResult> Get([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, Guid subjectId)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new GetAllLessonsInDateRangeQuery(userId, fromDate, toDate));
        return Ok(result);
    }
}