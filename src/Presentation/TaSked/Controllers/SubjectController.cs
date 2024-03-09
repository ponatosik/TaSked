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

public class SubjectController : ControllerBase
{
	private readonly IMediator _mediator;

	public SubjectController(IMediator mediator)
	{
		_mediator = mediator;
	}
	
	[HttpPost]
	[Route("Create")] 
	[Authorize(AccessPolicise.Moderator)]
	public async Task<IActionResult> Post(CreateSubjectRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		return Ok(await _mediator.Send(new CreateSubjectCommand(userId, request.SubjectName)));
	}

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new GetAllSubjectsQuery(userId)));
    }

    [HttpDelete]
    [Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Delete(DeleteSubjectRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        await _mediator.Send(new DeleteSubjectCommand(userId, request.SubjectId));
        return Ok();
    }

    [HttpPatch]
    [Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Patch(ChangeSubjectNameRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new ChangeSubjectNameCommand(userId, request.SubjectId, request.NewSubjectName)));
    }

    [HttpPatch]
    [Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Patch(ChangeSubjectTeacherRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new ChangeSubjectTeacherCommand(userId, request.SubjectId, request.NewSubjectTeacher)));
    }
}