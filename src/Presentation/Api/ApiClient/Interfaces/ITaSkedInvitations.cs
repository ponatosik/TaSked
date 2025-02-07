using Refit;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.Api.ApiClient;

public interface ITaSkedInvitations
{
	[Post("/Invitations")]
	public Task<Invitation> CreateInvitation([Body] CreateInvitationRequest request);

	[Post("/Invitations/Activate")]
	public Task ActivateInvitation([Body] ActivateInvitationRequest request);

	[Patch("/Invitations/Expire")]
	public Task ExpireInvitation([Body] ExpireInvitationRequest request);

	[Get("/Invitations/{invitationId}")]
	public Task<Invitation> GetInvitationById(Guid invitationId);

	[Get("/Invitations")]
	public Task<List<Invitation>> GetAllInvitation();
}
