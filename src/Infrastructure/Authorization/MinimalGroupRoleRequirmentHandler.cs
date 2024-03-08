using Microsoft.AspNetCore.Authorization;
using TaSked.Application.Data;
using TaSked.Domain;
using TaSked.Infrastructure.Authorization;

namespace Infrastructure.Authorization;

public class MinimalGroupRoleRequirmentHandler : AuthorizationHandler<MinimalGroupRoleRequirment>
{
	private readonly IApplicationDbContext _dbContext;
	public MinimalGroupRoleRequirmentHandler(IApplicationDbContext dbContext) 
	{
		_dbContext = dbContext;
	}

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimalGroupRoleRequirment requirement)
	{
		GroupRole requiredRole = requirement.GroupRole;
		Guid? userId = context.User.GetUserId();
		if (userId == null)
		{
			context.Fail();
			return Task.CompletedTask;
		}

		User? user = _dbContext.Users.FirstOrDefault(user => user.Id == userId);
		if (user == null)
		{
			context.Fail();
			return Task.CompletedTask;
		}
		if (user.Role < requiredRole)
		{
			context.Fail();
			return Task.CompletedTask;
		}

		context.Succeed(requirement);
		return Task.CompletedTask;
	}
}
