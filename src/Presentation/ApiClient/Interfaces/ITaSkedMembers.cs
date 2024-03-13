using Api.Requests;
using Refit;
using TaSked.Domain;

namespace TaSked.Api.ApiClient;

public interface ITaSkedMembers
{
	[Get("/Groups/{groupId}/Members")]
	public Task<List<User>> GetMembers(Guid groupId);

	[Patch("/Groups/{groupId}/Promote")]
	public Task<List<User>> PromoteMember(Guid groupId, [Body] PromoteMemberRequest request);

	[Patch("/Groups/{groupId}/Demote")]
	public Task<List<User>> DemoteMember(Guid groupId, [Body] DemoteMemberRequest request);

	[Delete("/Groups/{groupId}/Ban")]
	public Task<List<User>> BanMember(Guid groupId, [Body] BanMemberRequest request);
}
