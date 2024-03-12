using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class GetUserInfoHandler : IRequestHandler<GetUserInfoQuery, User>
{
    private readonly IApplicationDbContext _context;

    public GetUserInfoHandler(IApplicationDbContext context)
    {
        _context = context;
    }

	public Task<User> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
	{
		var user = _context.Users.FindById(request.UserId);

		return Task.FromResult(user);
	}
 }