using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

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
		if (await _context.Users.AnyAsync(u => u.Nickname == request.Nickname, cancellationToken))
		{
			throw new UserNicknameAlreadyTaken(request.Nickname);
		}
		
		var user = User.Create(request.Nickname);
		await _context.Users.AddAsync(user, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);
		return user;
	}  
}
