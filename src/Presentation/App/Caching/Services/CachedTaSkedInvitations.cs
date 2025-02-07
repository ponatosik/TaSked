using Akavache;
using Microsoft.Maui.Networking;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.App.Caching;

public class CachedTaSkedInvitations : CachedRepository<Invitation>, ITaSkedInvitations
{
	private readonly ITaSkedService _api;

	public CachedTaSkedInvitations(IBlobCache cache, ITaSkedService api, IConnectivity connectivity) : base(cache)
    {
	    _api = api;

	    if(connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			_ = FetchAndCacheEntities();
		}
    }

	public async Task<Invitation> CreateInvitation(CreateInvitationRequest request)
    {
		var invitation = await _api.CreateInvitation(request);
		await CacheEntityAsync(invitation);
		return invitation;
    }

	public async Task ActivateInvitation(ActivateInvitationRequest request)
	{
		await InvalidateEntityByKey(request.InvitationId.ToString());
		await _api.ActivateInvitation(request);
	}

	public async Task ExpireInvitation(ExpireInvitationRequest request)
	{
		await InvalidateEntityByKey(request.InvitationId.ToString());
		await _api.ExpireInvitation(request);
	}

	public async Task<Invitation> GetInvitationById(Guid InvitationId)
	{
		return await _api.GetInvitationById(InvitationId);
	}

	public async Task<List<Invitation>> GetAllInvitation()
	{
		var invitations = (await GetCachedEntities()).ToList();
		if(invitations.Count == 0) 
		{
			_ = await FetchAndCacheEntities();
		}
		return invitations.ToList();
	}

	protected override string GetEntityKey(Invitation entity)
	{
		return entity.Id.ToString();
	}

	protected override async Task<IEnumerable<Invitation>> FetchEntities()
	{
		return await _api.GetAllInvitation();
	}
}
