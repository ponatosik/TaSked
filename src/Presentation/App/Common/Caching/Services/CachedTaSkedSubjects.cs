using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Application;

namespace TaSked.App.Common.Caching;

public class CachedTaSkedSubjects : CachedRepository<SubjectDTO>, ITaSkedSubjects
{
	private readonly ITaSkedSevice _api;
	private readonly IConnectivity _connectivity;

	public CachedTaSkedSubjects(ITaSkedSevice api, IConnectivity connectivity)
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
		return (await GetCachedEntities()).ToList();
	}

	public async Task DeleteSubject(DeleteSubjectRequest request)
	{
		await _api.DeleteSubject(request);
		await InvalidateEntityByKey(request.SubjectId.ToString());
	}

	public async Task<UpdateSubjectDTO> ChangeSubjectName(ChangeSubjectNameRequest request)
	{
		var subject = await _api.ChangeSubjectName(request);
		await UpdateEntity(new SubjectDTO(subject.Id, subject.GroupId, subject.Name, 0, 0, subject.Teacher));
		return subject;
	}

	public async Task<UpdateSubjectDTO> ChangeSubjectTeacher(ChangeSubjectTeacherRequest request)
	{
		var subject = await _api.ChangeSubjectTeacher(request);
		await UpdateEntity(new SubjectDTO(subject.Id, subject.GroupId, subject.Name, 0, 0, subject.Teacher));
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
}
