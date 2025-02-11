using TaSked.Api.ApiClient;

namespace TaSked.App.Common;

public interface ITokenStore : IUserTokenStore, IRefreshTokenStore;