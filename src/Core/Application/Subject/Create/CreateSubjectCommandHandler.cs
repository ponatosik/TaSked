using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, SubjectDTO>
{
	private readonly IApplicationDbContext _context;

	public CreateSubjectCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<SubjectDTO> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
	{
		var user = _context.Users.FindById(request.UserId);
		var group = _context.Groups.FindById(user.GroupId.Value);

		var subject = group.CreateSubject(request.SubjectName, request.Teacher);

		await _context.SaveChangesAsync(cancellationToken);
		return SubjectDTO.From(subject);
	}
}
