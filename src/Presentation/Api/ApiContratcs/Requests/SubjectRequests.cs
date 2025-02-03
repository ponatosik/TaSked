using TaSked.Domain;

namespace TaSked.Api.Requests;

public record CreateSubjectRequest(string SubjectName, List<RelatedLink>? RelatedLinks = null);
public record DeleteSubjectRequest(Guid SubjectId);
public record ChangeSubjectNameRequest(Guid SubjectId, string NewSubjectName);

public record ChangeSubjectTeacherRequest(List<UpdateTeacherDTO> NewSubjectTeachers);

public record ChangeSubjectLinksRequest(Guid SubjectId, List<RelatedLink> NewLinks);

public record CommentSubjectRequest(Guid SubjectId, string Comment);
