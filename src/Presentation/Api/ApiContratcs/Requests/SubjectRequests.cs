using TaSked.Domain;

namespace TaSked.Api.Requests;

public record CreateSubjectRequest(string SubjectName, Teacher Teacher);
public record DeleteSubjectRequest(Guid SubjectId);
public record ChangeSubjectNameRequest(Guid SubjectId, string NewSubjectName);

public record ChangeSubjectTeacherRequest(Guid SubjectId, Teacher? NewSubjectTeacher);