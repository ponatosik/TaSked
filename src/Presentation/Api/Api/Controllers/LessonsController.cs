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
public class LessonsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LessonsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Post(CreateLessonRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new CreateLessonCommand(userId, request.SubjectId, request.LessonTime));
		return CreatedAtAction(nameof(Get), new { SubjectId = result.SubjectId }, result);
    }

    [HttpDelete]
    [Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Delete(DeleteLessonRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        await _mediator.Send(new DeleteLessonCommand(userId, request.SubjectId, request.LessonId));
        return NoContent();
    }

    [HttpPatch]
    [Authorize(AccessPolicise.Moderator)]
    [Route("Time")]
    public async Task<IActionResult> Patch(ChangeLessonTimeRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new ChangeLessonTimeCommand(userId, request.SubjectId, request.LessonId, request.NewTime));
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