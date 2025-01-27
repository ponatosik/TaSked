using System.Security.Claims;

namespace TaSked.Infrastructure.Authorization;

public static class ClaimsPrincipalExtensions
{
	public static Guid? GetUserId(this ClaimsPrincipal claims)
	{
		if (!claims.HasClaim(c => c.Type == CustomClaimTypes.UserId))
		{
			return null;
		}
		
		var userIdStr = claims.FindFirst(CustomClaimTypes.UserId)!.Value;
		if (Guid.TryParse(userIdStr, out var userId))
		{
			return userId;
		}

		return null;
	}
}
