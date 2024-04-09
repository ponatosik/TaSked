using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Domain;
using Microsoft.Maui.Networking;

namespace TaSked.App.Common.Caching;


public class CachedTaSkedHomeworks : CachedRepository<Homework>, ITaSkedHomeworks
{
	private readonly ITaSkedSevice _api;
	private readonly IConnectivity _connectivity;
	private readonly Task _initializing;

	public CachedTaSkedHomeworks(ITaSkedSevice api, IConnectivity connectivity)
	{
		_api = api;
		_connectivity = connectivity;

		if(_connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			_initializing = FetchAndCacheEntities();
		}
		else
		{
			_initializing = Task.CompletedTask;
		}
	}

	public async Task<List<Homework>> GetAllHomework()
	{
		await _initializing;
		return (await GetCachedEntities()).ToList();
	}

	public async Task<Homework> CreateHomework(CreateHomeworkRequest request)
	{
		Homework homework = await _api.CreateHomework(request);
		await CacheEntityAsync(homework);
		return homework;
	}

	public async Task DeleteHomework(DeleteHomeworkRequest request)
	{
		await _api.DeleteHomework(request);
		await InvalidateEntityByKey(request.HomeworkId.ToString());
	}

	public async Task<Homework> ChangeDeadline(ChangeHomeworkDeadlineRequest request)
	{
		Homework homework = await _api.ChangeDeadline(request);
		await UpdateEntity(homework);
		return homework;
	}

	public async Task<Homework> ChangeDescription(ChangeHomeworkDescriptionRequest request)
	{
		Homework homework = await _api.ChangeDescription(request);
		await UpdateEntity(homework);
		return homework;
	}

	public async Task<Homework> ChangeSourceUrl(ChangeHomeworkSourceUrlRequest request)
	{
		Homework homework = await _api.ChangeSourceUrl(request);
		await UpdateEntity(homework);
		return homework;
	}

	public async Task<Homework> ChangeTitle(ChangeHomeworkTitleRequest request)
	{
		Homework homework = await _api.ChangeTitle(request);
		await UpdateEntity(homework);
		return homework;
	}

	protected override string GetEntityKey(Homework entity)
	{
		return entity.Id.ToString();
	}

	protected override async Task<IEnumerable<Homework>> FetchEntities()
	{
		return await _api.GetAllHomework();
	}
}
