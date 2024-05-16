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

public class SubjectsController : ControllerBase
{
	private readonly IMediator _mediator;

	public SubjectsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	[Authorize(AccessPolicise.Moderator)]
	public async Task<IActionResult> Post(CreateSubjectRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new CreateSubjectCommand(userId, request.SubjectName, request.Teacher));
		return CreatedAtAction(nameof(Get), new { }, result);
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new GetAllSubjectsQuery(userId));
		return Ok(result);
	}

	[HttpDelete]
	[Authorize(AccessPolicise.Moderator)]
	public async Task<IActionResult> Delete(DeleteSubjectRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		await _mediator.Send(new DeleteSubjectCommand(userId, request.SubjectId));
		return NoContent();
	}

	[HttpPatch]
	[Authorize(AccessPolicise.Moderator)]
	[Route("Name")]
	public async Task<IActionResult> Patch(ChangeSubjectNameRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new ChangeSubjectNameCommand(userId, request.SubjectId, request.NewSubjectName));
		return Ok(result);
	}

	[HttpPatch]
	[Authorize(AccessPolicise.Moderator)]
	[Route("Teacher")]
	public async Task<IActionResult> Patch(ChangeSubjectTeacherRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new ChangeSubjectTeacherCommand(userId, request.SubjectId, request.NewSubjectTeacher));
		return Ok(result);
	}
}