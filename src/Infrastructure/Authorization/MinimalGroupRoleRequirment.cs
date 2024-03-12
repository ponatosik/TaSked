using Microsoft.AspNetCore.Authorization;
using TaSked.Domain;

namespace Infrastructure.Authorization;

public class MinimalGroupRoleRequirment : IAuthorizationRequirement
{
	public GroupRole GroupRole { get; }

	public MinimalGroupRoleRequirment(GroupRole role) 
	{
		GroupRole = role;
	}
}
