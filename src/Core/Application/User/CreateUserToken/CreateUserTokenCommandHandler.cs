﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

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
		if (await _context.Users.AnyAsync(u => u.Nickname == request.Nickname, cancellationToken))
		{
			throw new UserNicknameAlreadyTaken(request.Nickname);
		}
		
		var user = User.Create(request.Nickname);
		await _context.Users.AddAsync(user, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		var token = _jwtProvider.Generate(user);
		return token;
	}  
}
