using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
	private readonly IApplicationDbContext _context;

	public CreateUserCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
	{
		var user = User.Create(request.Nickname);
		await _context.Users.AddAsync(user, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);
		return user;
	}  
}
