using System.ComponentModel.DataAnnotations;

namespace TaSked.Infrastructure.Authorization;

public class JwtOptions
{
	public const string SectionName = "Authorization:Jwt";

	[Required]
	[MinLength(5)]
	public required string ValidIssuers { get; init; }

	[Required]
	[MinLength(5)]
	public required string ValidAudiences { get; init; }

	[Required]
	[MinLength(5)]
	public required string Issuer { get; init; }

	[Required]
	[MinLength(5)]
	public required string Audience { get; init; }

	[Required]
	[MinLength(32)]
	public required string SecretKey { get; init; }

	[Required]
	[Range(1, long.MaxValue)]
	public required long Duration { get; init; } // In seconds

	public bool ValidateLifetime { get; init; } = true;
}
