using Akavache;
using Microsoft.Maui.Networking;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.App.Caching;

public class CachedTaSkedLessons : CachedRepository<Lesson>, ITaSkedLessons
{
	private readonly ITaSkedService _api;

	public CachedTaSkedLessons(IBlobCache cache, ITaSkedService api, IConnectivity connectivity) : base(cache)
	{
		_api = api;

		if (connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			_ = FetchAndCacheEntities();
		}
	}

	public async Task<Lesson> CreateLesson(CreateLessonRequest request, Guid subjectId)
	{
		var lesson = await _api.CreateLesson(request, subjectId);
		await CacheEntityAsync(lesson);
		return lesson;
	}

	public async Task DeleteLesson(Guid subjectId, Guid lessonId)
	{
		await _api.DeleteLesson(subjectId, lessonId);
		await InvalidateEntityByKey(lessonId.ToString());
	}

	public async Task<Lesson> ChangeLessonTime(ChangeLessonTimeRequest request, Guid subjectId, Guid lessonId)
	{
		var lesson = await _api.ChangeLessonTime(request, subjectId, lessonId);
		await UpdateEntity(lesson);
		return lesson;
	}

	public async Task<Lesson> ChangeLessonLink(ChangeLessonLinkRequest request, Guid subjectId, Guid lessonId)
	{
		var lesson = await _api.ChangeLessonLink(request, subjectId, lessonId);
		await UpdateEntity(lesson);
		return lesson;
	}

	public async Task<List<Lesson>> GetSubjectLessons(Guid subjectId)
	{
		IEnumerable<Lesson> allLessons = await GetCachedEntities();
		return allLessons.Where(lesson => lesson.SubjectId == subjectId).ToList();
	}

	public async Task<List<Lesson>> Get(DateTime? from = null, DateTime? to = null)
	{
		var allLessons = (await GetCachedEntities()).ToList();
		if(allLessons.Count == 0) 
		{
			allLessons = (await FetchAndCacheEntities()).ToList();
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
