using Refit;
using TaSked.Api.Requests;
using TaSked.Application;

namespace TaSked.Api.ApiClient;

public interface ITaSkedGroups
{
	[Post("/Groups")]
	public Task<GroupDTO> CreateGroup([Body] CreateGroupRequest request);

	[Delete("/Groups")]
	public Task DeleteGroup();

	[Patch("/Groups/Name")]
	public Task<GroupDTO> ChangeGroupName([Body] ChangeGroupNameRequest request);

	[Patch("/Groups/Leave")]
	public Task LeaveGroup();

	[Get("/Groups/{groupId}")]
	public Task<GroupDTO> GetGroupById(Guid groupId);
}
