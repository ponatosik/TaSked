﻿using TaSked.Domain;
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
    [Route("Time")]
    public async Task<IActionResult> Patch(ChangeLessonTimeRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new ChangeLessonTimeCommand(userId, request.SubjectId, request.LessonId, request.NewTime)));
    }

    [HttpGet]
    [Route("BySubject/{subjectId:guid}")]
    public async Task<IActionResult> Get(Guid subjectId)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new GetAllLessonsBySubjectQuery(userId, subjectId)));
    }

    [HttpGet]
    [Route("ByDateRange")]
    public async Task<IActionResult> Get([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new GetAllLessonsInDateRangeQuery(userId, fromDate, toDate)));
    }
}