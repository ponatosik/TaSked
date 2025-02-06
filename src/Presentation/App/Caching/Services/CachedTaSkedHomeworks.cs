using Akavache;
using Microsoft.Maui.Networking;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.App.Caching;


public class CachedTaSkedHomeworks : CachedRepository<Homework>, ITaSkedHomeworks
{
	private readonly ITaSkedService _api;

	public CachedTaSkedHomeworks(IBlobCache cache, ITaSkedService api, IConnectivity connectivity) : base(cache)
	{
		_api = api;

		if(connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			_ = FetchAndCacheEntities();
		}
	}

	public async Task<List<Homework>> GetAllHomework()
	{
		var homeworks = (await GetCachedEntities()).ToList();
		if(homeworks.Count == 0) 
		{
			homeworks = (await FetchAndCacheEntities()).ToList();
		}
		return homeworks;
	}
	
	public async Task<Homework> CreateHomework(CreateHomeworkRequest request, Guid subjectId)
	{
		Homework homework = await _api.CreateHomework(request, subjectId);
		await CacheEntityAsync(homework);
		return homework;
	}

	public async Task DeleteHomework(Guid subjectId, Guid homeworkId)
	{
		await _api.DeleteHomework(subjectId, homeworkId);
		await InvalidateEntityByKey(homeworkId.ToString());
	}

	public async Task<Homework> ChangeHomeworkDeadline(ChangeHomeworkDeadlineRequest request, Guid subjectId, Guid homeworkId)
	{
		var homework = await _api.ChangeHomeworkDeadline(request, subjectId, homeworkId);
		await UpdateEntity(homework);
		return homework;
	}

	public async Task<Homework> ChangeHomeworkDescription(ChangeHomeworkDescriptionRequest request, Guid subjectId, Guid homeworkId)
	{
		var homework = await _api.ChangeHomeworkDescription(request, subjectId, homeworkId);
		await UpdateEntity(homework);
		return homework;
	}

	public async Task<Homework> ChangeHomeworkSourceUrl(ChangeHomeworkRelatedLinksRequest request, Guid subjectId, Guid homeworkId)
	{
		var homework = await _api.ChangeHomeworkSourceUrl(request, subjectId, homeworkId);
		await UpdateEntity(homework);
		return homework;
	}

	public async Task<Homework> ChangeHomeworkTitle(ChangeHomeworkTitleRequest request, Guid subjectId, Guid homeworkId)
	{
		var homework = await _api.ChangeHomeworkTitle(request, subjectId, homeworkId);
		await UpdateEntity(homework);
		return homework;
	}

	public async Task<Homework> ChangeHomeworkBriefSummary(ChangeHomeworkBriefSummaryRequest request, Guid subjectId, Guid homeworkId)
	{
		var homework = await _api.ChangeHomeworkBriefSummary(request, subjectId, homeworkId);
		await UpdateEntity(homework);
		return homework;
	}

	public Task<List<CommentDTO>> GetHomeworkComments(Guid subjectId, Guid homeworkId)
	{
		return _api.GetHomeworkComments(subjectId, homeworkId);
	}

	public Task<CommentDTO> CommentHomework(CommentHomeworkRequest request, Guid subjectId, Guid homeworkId)
	{
		return _api.CommentHomework(request, subjectId, homeworkId);
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
