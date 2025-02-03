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

public class SubjectsController : ControllerBase
{
	private readonly IMediator _mediator;

	public SubjectsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	[Authorize(AccessPolicies.Moderator)]
	public async Task<IActionResult> Post(CreateSubjectRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(
			new CreateSubjectCommand(userId, request.SubjectName, request.Teacher, request.RelatedLinks));
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
	[Authorize(AccessPolicies.Moderator)]
	public async Task<IActionResult> Delete(DeleteSubjectRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		await _mediator.Send(new DeleteSubjectCommand(userId, request.SubjectId));
		return NoContent();
	}

	[HttpPatch]
	[Authorize(AccessPolicies.Moderator)]
	[Route("Name")]
	public async Task<IActionResult> Patch(ChangeSubjectNameRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new ChangeSubjectNameCommand(userId, request.SubjectId, request.NewSubjectName));
		return Ok(result);
	}

	[HttpPatch]
	[Authorize(AccessPolicies.Moderator)]
	[Route("Links")]
	public async Task<IActionResult> Patch(ChangeSubjectLinksRequest request)
	{
		var userId = User.GetUserId()!.Value;
		var result =
			await _mediator.Send(new ChangeSubjectRelatedLinksCommand(userId, request.SubjectId, request.NewLinks));
		return Ok(result);
	}

	[HttpPatch]
	[Authorize(AccessPolicies.Moderator)]
	[Route("Teacher")]
	public async Task<IActionResult> Patch(ChangeSubjectTeacherRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new ChangeSubjectTeacherCommand(userId, request.SubjectId, request.NewSubjectTeacher));
		return Ok(result);
	}

	[HttpGet]
	[Route("{subjectId:guid}/Homeworks/{homeworkId:guid}/Comments")]
	public async Task<IActionResult> Get(Guid subjectId, Guid homeworkId)
	{
		var userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new GetHomeworkCommentsQuery(userId, subjectId, homeworkId));
		return Ok(result);
	}

	[HttpGet]
	[Route("{subjectId:guid}/Comments")]
	public async Task<IActionResult> Get(Guid subjectId)
	{
		var userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new GetSubjectCommentsQuery(userId, subjectId));
		return Ok(result);
	}


	[HttpPost]
	[Route("Comments")]
	public async Task<IActionResult> Post(CommentSubjectRequest request)
	{
		var userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new CommentSubjectCommand(userId, request.SubjectId, request.Comment));
		return Ok(result);
	}

	[HttpPost]
	[Route("{subjectId:guid}/Homeworks/{homeworkId:guid}/Comments")]
	public async Task<IActionResult> Post(Guid subjectId, Guid homeworkId, CommentHomeworkRequest request)
	{
		var userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(
			new CommentHomeworkCommand(userId, subjectId, homeworkId, request.Content));
		return Ok(CommentDTO.From(result));
	}
}