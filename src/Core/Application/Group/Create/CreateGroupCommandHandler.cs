using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, GroupDTO>
{
	private readonly IApplicationDbContext _context;

	public CreateGroupCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<GroupDTO> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
	{
		var user = _context.Users.FindById(request.CreatorId);

		var group = Group.Create(request.GroupName, user);
		await _context.Groups.AddAsync(group, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		return GroupDTO.From(group);
	}  
}
