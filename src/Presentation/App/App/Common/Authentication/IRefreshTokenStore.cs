namespace TaSked.App.Common;

public interface IRefreshTokenStore
{
	public string? RefreshToken { get; set; }
}