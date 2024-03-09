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
public class LessonController : ControllerBase
{
    private readonly IMediator _mediator;

    public LessonController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Post(CreateLessonRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new CreateLessonCommand(userId, request.SubjectId, request.LessonTime)));
    }

    [HttpDelete]
    [Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Delete(DeleteLessonRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        await _mediator.Send(new DeleteLessonCommand(userId, request.SubjectId, request.LessonId));
        return Ok();
    }

    [HttpPatch]
    [Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Patch(ChangeLessonTimeRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new ChangeLessonTimeCommand(userId, request.SubjectId, request.LessonId, request.NewTime)));
    }

    [HttpGet]
    public async Task<IActionResult> Get(GetAllLessonsBySubjectRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new GetAllLessonsBySubjectQuery(userId, request.SubjectId)));
    }

    [HttpGet]
    public async Task<IActionResult> Get(GetAllLessonsInDateRangeRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new GetAllLessonsInDateRangeQuery(userId, request.StartDate, request.EndDate)));
    }
}