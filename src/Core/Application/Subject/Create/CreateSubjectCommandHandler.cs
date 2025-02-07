using MediatR;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, SubjectDTO>
{
	private readonly IApplicationDbContext _context;
	private readonly IPublisher? _eventPublisher;

	public CreateSubjectCommandHandler(IApplicationDbContext context, IPublisher? eventPublisher = null)
	{
		_context = context;
		_eventPublisher = eventPublisher;
	}

	public async Task<SubjectDTO> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
	{
		var user = _context.Users.FindOrThrow(request.UserId);
		var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
		var group = _context.Groups.FindOrThrow(groupId);

		var subject = group.CreateSubject(request.SubjectName, request.Teachers);
		subject.RelatedLinks.AddRange(request.RelatedLinks ?? []);
		
		var result = SubjectDTO.From(subject);

		await _context.SaveChangesAsync(cancellationToken);
		if(_eventPublisher != null)
		{
			await _eventPublisher.Publish(new SubjectCreatedEvent(result, group.Id), cancellationToken);
		}
		return result;
	}
}
