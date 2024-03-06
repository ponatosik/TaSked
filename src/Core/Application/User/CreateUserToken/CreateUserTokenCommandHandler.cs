using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class CreateUserTokenCommandHandler : IRequestHandler<CreateUserTokenCommand, string>
{
	private readonly IApplicationDbContext _context;
	private readonly IJwtProvider _jwtProvider;

	public CreateUserTokenCommandHandler(IApplicationDbContext context, IJwtProvider jwtProvider)
	{
		_context = context;
		_jwtProvider = jwtProvider;
	}

	public async Task<string> Handle(CreateUserTokenCommand request, CancellationToken cancellationToken)
	{
		var user = User.Create(request.Nickname);
		await _context.Users.AddAsync(user, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		string token = _jwtProvider.Generate(user);
		return token;
	}  
}
