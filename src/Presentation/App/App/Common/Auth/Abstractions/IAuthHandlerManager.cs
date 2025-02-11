namespace TaSked.App.Common;

public interface IAuthHandlerManager
{
	Task<IAuthHandler?> LoadCurrentAuthHandlerAsync();
	Task SaveCurrentAuthHandlerAsync(IAuthHandler authHandler);
	Task RemoveCurrentAuthHandlerAsync();
}