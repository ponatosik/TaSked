
using System.Net.Http.Headers;

namespace TaSked.Api.ApiClient;

public class HttpMessageHandler : DelegatingHandler
{
	private readonly IUserTokenStore _tokenStore;

	public HttpMessageHandler(IUserTokenStore tokenStore) : base()
	{
		_tokenStore = tokenStore;
	}

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		string? token = _tokenStore.AccessToken;
		if (token != null)
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenStore.AccessToken);
		}

		return base.SendAsync(request, cancellationToken);
	}
}
