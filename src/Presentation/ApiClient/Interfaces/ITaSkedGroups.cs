using Refit;
using TaSked.Api.Requests;
using TaSked.Application;

namespace TaSked.Api.ApiClient;

public interface ITaSkedGroups
{
	[Post("/Groups")]
	public Task<GroupDTO> CreateGroup(CreateGroupRequest request);

	[Delete("/Groups")]
	public Task DeleteGroup();

	[Patch("/Groups/Name")]
	public Task<GroupDTO> ChangeGroupName(ChangeGroupNameRequest request);

	[Patch("/Groups/Leave")]
	public Task LeaveGroup();

	[Get("/Groups/{GroupId}")]
	public Task<GroupDTO> GetGroupById(Guid GroupId);
}
