using TaSked.Domain;

namespace TaSked.Api.Requests;

public record CreateSubjectRequest(string SubjectName, List<RelatedLink>? RelatedLinks = null);

public record ChangeSubjectNameRequest(string NewSubjectName);

public record ChangeSubjectTeachersRequest(List<UpdateTeacherDTO> NewSubjectTeachers);

public record ChangeSubjectLinksRequest(List<RelatedLink> NewLinks);

public record CommentSubjectRequest(string Comment);
