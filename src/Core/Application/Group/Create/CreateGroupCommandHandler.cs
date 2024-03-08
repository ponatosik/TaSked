using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Group>
{
	private readonly IApplicationDbContext _context;

	public CreateGroupCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Group> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
	{
		var user = _context.Users.FindById(request.CreatorId);

		var group = Group.Create(request.GroupName, user);
		await _context.Groups.AddAsync(group, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		return group;
	}  
}
