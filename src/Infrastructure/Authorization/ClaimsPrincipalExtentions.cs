using System.Security.Claims;

namespace TaSked.Infrastructure.Authorization;

public static class ClaimsPrincipalExtentions
{
	public static Guid? GetUserId(this ClaimsPrincipal claims)
	{
		string? id = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (id == null) 
		{ 
			return null; 
		}

		return Guid.Parse(id);
	}
}
