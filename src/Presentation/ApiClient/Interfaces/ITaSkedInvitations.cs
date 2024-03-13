using Refit;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.Api.ApiClient;

public interface ITaSkedInvitations
{
	[Post("/Invitations/")]
	public Task<Invitation> CreateInvitation(CreateInvintationRequest request);

	[Post("/Invitations/Activate")]
	public Task ActivateInvitation(ActivateInvintationRequest request);

	[Patch("/Invitations/Expire")]
	public Task ExpireInvitation(ExpireInvintationRequest request);

	[Get("/Invitations/{InvitationId}")]
	public Task<Invitation> GetInvitationById(Guid InvitationId);
}
