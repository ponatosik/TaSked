//using Api.Requests;
//using TaSked.Api.ApiClient;
//using TaSked.Domain;

//namespace TaSked.App.Caching;

//public class CachedTaSkedMembers : ITaSkedMembers, CachedRepository<ITaSkedMembers>
//{
//	public Task<List<User>> GetMembers(Guid groupId);

//	public Task<List<User>> PromoteMember(Guid groupId, PromoteMemberRequest request);

//	public Task<List<User>> DemoteMember(Guid groupId, DemoteMemberRequest request);

//	public Task<List<User>> BanMember(Guid groupId, BanMemberRequest request);

//	public Task ClearCache()
//	{
//		throw new NotImplementedException();
//	}
//}
