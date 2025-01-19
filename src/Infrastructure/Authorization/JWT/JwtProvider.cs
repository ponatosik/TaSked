using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.Infrastructure.Authorization;

public class JwtProvider : IJwtProvider
{
	private readonly JwtOptions _options;

	public JwtProvider(IOptions<JwtOptions> options)
	{
		_options = options.Value;
	}

	public string Generate(User user)
	{
		var credentials = GenerateCredentials();
		Claim[] claims =
		[
			new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
		];


		var token = new JwtSecurityToken(
			_options.Issuer,
			_options.Audience,
			expires: DateTime.UtcNow.AddSeconds(_options.Duration),
			claims: claims,
			signingCredentials: credentials
			);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	private SigningCredentials GenerateCredentials()
	{
		return new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
			SecurityAlgorithms.HmacSha256);
	}
}
