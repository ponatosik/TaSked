using System.Security.Claims;

namespace TaSked.Infrastructure.Authorization;

public static class ClaimsPrincipalExtensions
{
	public static Guid? GetUserId(this ClaimsPrincipal claims)
	{
		var id = claims.FindFirstValue("https://tasked.com/user_id");
		id ??= claims.FindFirstValue(ClaimTypes.NameIdentifier);

		if (Guid.TryParse(id, out var userId))
		{
			return userId;
		}

		return null;
	}
}
