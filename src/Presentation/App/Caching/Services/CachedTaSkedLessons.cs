using Akavache;
using Microsoft.Maui.Networking;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.App.Caching;

public class CachedTaSkedLessons : CachedRepository<Lesson>, ITaSkedLessons
{
	private readonly ITaSkedSevice _api;
	private readonly IConnectivity _connectivity;

	public CachedTaSkedLessons(IBlobCache cache, ITaSkedSevice api, IConnectivity connectivity) : base(cache)
	{
		_api = api;
		_connectivity = connectivity;

		if (_connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			FetchAndCacheEntities();
		}
	}

	public async Task<Lesson> Create(CreateLessonRequest request)
	{
		Lesson lesson = await _api.Create(request);
		await CacheEntityAsync(lesson);
		return lesson;
	}

	public async Task Delete(DeleteLessonRequest request)
	{
		await _api.Delete(request);
		await InvalidateEntityByKey(request.LessonId.ToString());
	}

	public async Task<Lesson> ChangeTime(ChangeLessonTimeRequest request)
	{
		Lesson lesson = await _api.ChangeTime(request);
		await UpdateEntity(lesson);
		return lesson;
	}

	public async Task<Lesson> ChangeLessonLink(ChangeLessonLinkRequest request)
	{
		var lesson = await _api.ChangeLessonLink(request);
		await UpdateEntity(lesson);
		return lesson;
	}

	public async Task<List<Lesson>> GetBySubject(Guid SubjectId)
	{
		IEnumerable<Lesson> allLessons = await GetCachedEntities();
		return allLessons.Where(lesson => lesson.SubjectId == SubjectId).ToList();
	}

	public async Task<List<Lesson>> Get(DateTime? from = null, DateTime? to = null)
	{
		var allLessons = await GetCachedEntities();
		if(!allLessons.Any()) 
		{
			allLessons = await FetchAndCacheEntities();
		}
		return allLessons.Where(lesson => lesson.Time <= to && lesson.Time >= from).ToList();
	}

	protected override string GetEntityKey(Lesson entity)
	{
		return entity.Id.ToString();
	}

	protected override async Task<IEnumerable<Lesson>> FetchEntities()
	{
		return await _api.Get();
	}
}
