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
			new CreateSubjectCommand(userId, request.SubjectName, [], request.RelatedLinks));
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
	[Route("{subjectId:guid}")]
	public async Task<IActionResult> Delete(Guid subjectId)
	{
		Guid userId = User.GetUserId()!.Value;
		await _mediator.Send(new DeleteSubjectCommand(userId, subjectId));
		return NoContent();
	}

	[HttpPatch]
	[Authorize(AccessPolicies.Moderator)]
	[Route("{subjectId:guid}/Name")]
	public async Task<IActionResult> Patch(ChangeSubjectNameRequest request, Guid subjectId)
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new ChangeSubjectNameCommand(userId, subjectId, request.NewSubjectName));
		return Ok(result);
	}

	[HttpPatch]
	[Authorize(AccessPolicies.Moderator)]
	[Route("{subjectId:guid}/Links")]
	public async Task<IActionResult> Patch(ChangeSubjectLinksRequest request, Guid subjectId)
	{
		var userId = User.GetUserId()!.Value;
		var result =
			await _mediator.Send(new ChangeSubjectRelatedLinksCommand(userId, subjectId, request.NewLinks));
		return Ok(result);
	}

	[HttpPatch]
	[Authorize(AccessPolicies.Moderator)]
	[Route("{subjectId:guid}/Teacher")]
	public async Task<IActionResult> Patch(Guid subjectId, ChangeSubjectTeachersRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(
			new ChangeSubjectTeachersCommand(userId, subjectId, request.NewSubjectTeachers));
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
	[Route("{subjectId:guid}/Comments")]
	public async Task<IActionResult> Post(CommentSubjectRequest request, Guid subjectId)
	{
		var userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new CommentSubjectCommand(userId, subjectId, request.Comment));
		return CreatedAtAction(nameof(Get), new { subjectId }, CommentDTO.From(result));
	}

}