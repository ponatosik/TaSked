using TaSked.Application;
using TaSked.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TaSked.Infrastructure.Authorization;

public class JwtProvider : IJwtProvider
{
	private readonly JwtOptions options;

	public JwtProvider(JwtOptions options)
	{
		this.options = options;
	}

	public string Generate(User user)
	{
		var credentionals = GenerateCredatials();
		Claim[] claims =
		[
			new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
		];


		var token = new JwtSecurityToken(
			issuer: options.Issuer, 
			audience: options.Audience, 
			claims: claims, 
			signingCredentials: credentionals
			);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	private SigningCredentials GenerateCredatials()
	{
		return new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
			SecurityAlgorithms.HmacSha256);
	}
}
