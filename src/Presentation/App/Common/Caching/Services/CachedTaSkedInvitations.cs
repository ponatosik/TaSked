using System.Runtime.InteropServices;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.App.Common.Caching;

public class CachedTaSkedInvitations : CachedRepository<Invitation>, ITaSkedInvitations
{
	private readonly ITaSkedSevice _api;

    public CachedTaSkedInvitations(ITaSkedSevice api) 
    {
		_api = api;
    }

    public async Task<Invitation> CreateInvitation(CreateInvintationRequest request)
    {
		var invitation = await _api.CreateInvitation(request);
		await CacheEntityAsync(invitation);
		return invitation;
    }

	public async Task ActivateInvitation(ActivateInvintationRequest request)
	{
		await InvalidateEntityByKey(request.InvitationId.ToString());
		await _api.ActivateInvitation(request);
	}

	public async Task ExpireInvitation(ExpireInvintationRequest request)
	{
		await InvalidateEntityByKey(request.InvitationId.ToString());
		await _api.ExpireInvitation(request);
	}

	public async Task<Invitation> GetInvitationById(Guid InvitationId)
	{
		return await GetCachedEntityAsync(InvitationId.ToString());
	}

	public async Task<List<Invitation>> GetAllInvitation()
	{
		return (await GetCachedEntities()).ToList();
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
