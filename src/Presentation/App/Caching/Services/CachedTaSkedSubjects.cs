using Akavache;
using Microsoft.Maui.Networking;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Application;

namespace TaSked.App.Caching;

public class CachedTaSkedSubjects : CachedRepository<SubjectDTO>, ITaSkedSubjects
{
	private readonly ITaSkedService _api;

	public CachedTaSkedSubjects(IBlobCache cache, ITaSkedService api, IConnectivity connectivity) : base(cache)
	{
		_api = api;

		if(connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			_ = FetchAndCacheEntities();
		}
	}

	public async Task<SubjectDTO> CreateSubject(CreateSubjectRequest request)
	{
		var subject = await _api.CreateSubject(request);
		await CacheEntityAsync(subject);
		return subject;
	}

	public async Task<List<SubjectDTO>> GetAllSubjects()
	{
		var subjects = (await GetCachedEntities()).ToList();
		if(subjects.Count == 0) 
		{
			subjects = (await FetchAndCacheEntities()).ToList();
		}
		return subjects.ToList();
	}

	public async Task DeleteSubject(Guid subjectId)
	{
		await _api.DeleteSubject(subjectId);
		await InvalidateEntityByKey(subjectId.ToString());
	}

	public async Task<UpdateSubjectDTO> ChangeSubjectName(ChangeSubjectNameRequest request, Guid subjectId)
	{
		var subject = await _api.ChangeSubjectName(request, subjectId);
		await UpdateEntity(ToSubjectDto(subject));
		return subject;
	}

	public async Task<UpdateSubjectDTO> ChangeSubjectLinks(ChangeSubjectLinksRequest request, Guid subjectId)
	{
		var subject = await _api.ChangeSubjectLinks(request, subjectId);
		await UpdateEntity(ToSubjectDto(subject));

		return subject;
	}

	public async Task<UpdateSubjectDTO> ChangeSubjectTeacher(ChangeSubjectTeachersRequest request, Guid subjectId)
	{
		var subject = await _api.ChangeSubjectTeacher(request, subjectId);
		await UpdateEntity(ToSubjectDto(subject));
		return subject;
	}

	public Task<List<CommentDTO>> GetSubjectComments(Guid subjectId)
	{
		return _api.GetSubjectComments(subjectId);
	}

	public Task<CommentDTO> CommentSubject(CommentSubjectRequest request, Guid subjectId)
	{
		return _api.CommentSubject(request, subjectId);
	}

	protected override string GetEntityKey(SubjectDTO entity)
	{
		return entity.Id.ToString();
	}

	protected override async Task<IEnumerable<SubjectDTO>> FetchEntities()
	{
		return await _api.GetAllSubjects();
	}

	private static SubjectDTO ToSubjectDto(UpdateSubjectDTO subject)
	{
		return new SubjectDTO(
			subject.Id,
			subject.GroupId,
			subject.Name,
			0,
			0,
			subject.Teachers,
			subject.RelatedLinks);
	}
}
