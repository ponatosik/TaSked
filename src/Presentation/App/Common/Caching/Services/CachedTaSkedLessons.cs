//using TaSked.Api.ApiClient;
//using TaSked.Api.Requests;
//using TaSked.Domain;

//namespace TaSked.App.Common.Caching;

//public class CachedTaSkedLessons : ITaSkedLessons, CachedRepository<ITaSkedLessons>
//{
//	public Task<Lesson> Create(CreateLessonRequest request);

//	public Task Delete(DeleteLessonRequest request);

//	public Task<Lesson> ChangeTime(ChangeLessonTimeRequest request);

//	public Task<List<Lesson>> GetBySubject(Guid SubjectId);

//	public Task<List<Lesson>> Get(DateTime? from = null, DateTime? to = null);

//	public Task ClearCache()
//	{
//		throw new NotImplementedException();
//	}
//}
