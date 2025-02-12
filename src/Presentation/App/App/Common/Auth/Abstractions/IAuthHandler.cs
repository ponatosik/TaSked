namespace TaSked.App.Common;

public interface IAuthHandler
{
	public Task RestoreSessionAsync();
	public Task LogoutAsync();
}

public interface IAuthHandler<in TLoginRequest> : IAuthHandler
	where TLoginRequest : ILoginRequest
{
	public Task LoginAsync(TLoginRequest request);
}

public interface ILoginRequest;