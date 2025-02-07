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

	[Delete("/Subjects/{subjectId}")]
	public Task DeleteSubject(Guid subjectId);

	[Patch("/Subjects/{subjectId}/Name")]
	public Task<UpdateSubjectDTO> ChangeSubjectName([Body] ChangeSubjectNameRequest request, Guid subjectId);

	[Patch("/Subjects/{subjectId}/Links")]
	public Task<UpdateSubjectDTO> ChangeSubjectLinks([Body] ChangeSubjectLinksRequest request, Guid subjectId);

	[Patch("/Subjects/{subjectId}/Teacher")]
	public Task<UpdateSubjectDTO> ChangeSubjectTeacher([Body] ChangeSubjectTeachersRequest request, Guid subjectId);

	[Get("/Subjects/{subjectId}/Comments")]
	public Task<List<CommentDTO>> GetSubjectComments(Guid subjectId);

	[Post("/Subjects/{subjectId}/Comments")]
	public Task<CommentDTO> CommentSubject([Body] CommentSubjectRequest request, Guid subjectId);
}
