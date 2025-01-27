using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace TaSked.Infrastructure.Authorization;

public class CustomClaimsTransformation : IClaimsTransformation
{
	private readonly JwtOptions _options;

	public CustomClaimsTransformation(IOptions<JwtOptions> options)
	{
		_options = options.Value;
	}

	public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
	{
		var claimsIdentity = new ClaimsIdentity();

		if (!principal.HasClaim(c => c.Type == CustomClaimTypes.UserId))
		{
			if (principal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
			{
				var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
				claimsIdentity.AddClaim(new Claim(CustomClaimTypes.UserId, userId));
			}
		}

		principal.AddIdentity(claimsIdentity);
		return Task.FromResult(principal);
	}
}