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
		var user = await _context.Users.FindAsync(request.CreatorId, cancellationToken);
		if (user == null)
		{
			throw new Exception("No such user found");
		}

		var group = Group.Create(request.GroupName, user);
		await _context.Groups.AddAsync(group, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		return group;
	}  
}
