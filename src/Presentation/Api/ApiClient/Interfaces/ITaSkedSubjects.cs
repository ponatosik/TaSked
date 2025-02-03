using Refit;
using TaSked.Api.Requests;
using TaSked.Application;

namespace TaSked.Api.ApiClient;

public interface ITaSkedSubjects
{
	[Post("/Subjects")]
	public Task<SubjectDTO> CreateSubject(CreateSubjectRequest request);

	[Get("/Subjects")]
	public Task<List<SubjectDTO>> GetAllSubjects();

	[Delete("/Subjects")]
	public Task DeleteSubject([Body] DeleteSubjectRequest request);

	[Patch("/Subjects/Name")]
	public Task<UpdateSubjectDTO> ChangeSubjectName([Body] ChangeSubjectNameRequest request);

	[Patch("/Subjects/Links")]
	public Task<UpdateSubjectDTO> ChangeSubjectLinks([Body] ChangeSubjectLinksRequest request);

	[Patch("/Subjects/{subjectId:guid}/Teacher")]
	public Task<UpdateSubjectDTO> ChangeSubjectTeacher(Guid subjectId, [Body] ChangeSubjectTeacherRequest request);
}
