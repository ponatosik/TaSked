using Microsoft.AspNetCore.Authorization;
using TaSked.Application.Data;
using TaSked.Domain;
using TaSked.Infrastructure.Authorization;

namespace Infrastructure.Authorization;

public class MinimalGroupRoleRequirementHandler : AuthorizationHandler<MinimalGroupRoleRequirment>
{
	private readonly IApplicationDbContext _dbContext;

	public MinimalGroupRoleRequirementHandler(IApplicationDbContext dbContext) 
	{
		_dbContext = dbContext;
	}

	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimalGroupRoleRequirment requirement)
	{
		GroupRole requiredRole = requirement.GroupRole;
		Guid? userId = context.User.GetUserId();
		if (userId == null)
		{
			context.Fail();
			return;
		}

		User? user = await _dbContext.Users.FindAsync(userId.Value);
		if (user == null || user.Role < requiredRole)
		{
			context.Fail();
			return;
		}

		context.Succeed(requirement);
	}
}
