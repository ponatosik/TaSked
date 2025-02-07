using Api.Requests;
using Refit;
using TaSked.Domain;

namespace TaSked.Api.ApiClient;

public interface ITaSkedMembers
{
	[Get("/Groups/{groupId}/Members")]
	public Task<List<User>> GetGroupMembers(Guid groupId);

	[Patch("/Groups/{groupId}/Members/Promote")]
	public Task PromoteGroupMember([Body] PromoteMemberRequest request, Guid groupId);

	[Patch("/Groups/{groupId}/Members/Demote")]
	public Task DemoteGroupMember([Body] DemoteMemberRequest request, Guid groupId);

	[Delete("/Groups/{groupId}/Members/Ban")]
	public Task BanGroupMember([Body] BanMemberRequest request, Guid groupId);
}
