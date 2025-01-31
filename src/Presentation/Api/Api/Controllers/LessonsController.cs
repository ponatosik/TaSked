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
public class LessonsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LessonsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(AccessPolicies.Moderator)]
    public async Task<IActionResult> Post(CreateLessonRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(
	        new CreateLessonCommand(userId, request.SubjectId, request.LessonTime, request.LessonLink));
		return CreatedAtAction(nameof(Get), new { SubjectId = result.SubjectId }, result);
    }

    [HttpDelete]
    [Authorize(AccessPolicies.Moderator)]
    public async Task<IActionResult> Delete(DeleteLessonRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        await _mediator.Send(new DeleteLessonCommand(userId, request.SubjectId, request.LessonId));
        return NoContent();
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("Time")]
    public async Task<IActionResult> Patch(ChangeLessonTimeRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new ChangeLessonTimeCommand(userId, request.SubjectId, request.LessonId, request.NewTime));
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("Link")]
    public async Task<IActionResult> Patch(ChangeLessonLinkRequest request)
    {
	    var userId = User.GetUserId()!.Value;
	    var result = await _mediator.Send(
		    new ChangeLessonLinkCommand(userId, request.SubjectId, request.LessonId, request.NewLink));
	    return Ok(result);
    }


    [HttpGet]
    [Route("BySubject/{SubjectId:guid}")]
    public async Task<IActionResult> Get(Guid SubjectId)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new GetAllLessonsBySubjectQuery(userId, SubjectId));
        return Ok(result);
    }

    [HttpGet]
    [Route("ByDateRange")]
    public async Task<IActionResult> Get([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new GetAllLessonsInDateRangeQuery(userId, fromDate, toDate));
        return Ok(result);
    }
}