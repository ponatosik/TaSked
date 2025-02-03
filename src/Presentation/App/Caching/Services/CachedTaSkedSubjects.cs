using Akavache;
using Microsoft.Maui.Networking;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Application;

namespace TaSked.App.Caching;

public class CachedTaSkedSubjects : CachedRepository<SubjectDTO>, ITaSkedSubjects
{
	private readonly ITaSkedSevice _api;
	private readonly IConnectivity _connectivity;

	public CachedTaSkedSubjects(IBlobCache cache, ITaSkedSevice api, IConnectivity connectivity) : base(cache)
	{
		_api = api;
		_connectivity = connectivity;

		if(_connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			FetchAndCacheEntities();
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
		var subjects = await GetCachedEntities();
		if(!subjects.Any()) 
		{
			subjects = await FetchAndCacheEntities();
		}
		return subjects.ToList();
	}

	public async Task DeleteSubject(DeleteSubjectRequest request)
	{
		await _api.DeleteSubject(request);
		await InvalidateEntityByKey(request.SubjectId.ToString());
	}

	public async Task<UpdateSubjectDTO> ChangeSubjectName(ChangeSubjectNameRequest request)
	{
		var subject = await _api.ChangeSubjectName(request);
		await UpdateEntity(ToSubjectDto(subject));
		return subject;
	}

	public async Task<UpdateSubjectDTO> ChangeSubjectLinks(ChangeSubjectLinksRequest request)
	{
		var subject = await _api.ChangeSubjectLinks(request);
		await UpdateEntity(ToSubjectDto(subject));

		return subject;
	}

	public async Task<UpdateSubjectDTO> ChangeSubjectTeacher(Guid subjectId ,ChangeSubjectTeacherRequest request)
	{
		var subject = await _api.ChangeSubjectTeacher(subjectId, request);
		await UpdateEntity(ToSubjectDto(subject));
		return subject;
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
