using TaSked.Domain;

namespace TaSked.Api.Requests;

public record CreateHomeworkRequest(
	Guid SubjectId,
	string Title,
	string Description,
	DateTime? Deadline = null,
	List<RelatedLink>? RelatedLinks = null);

public record DeleteHomeworkRequest(Guid SubjectId, Guid HomeworkId);
public record ChangeHomeworkDeadlineRequest(Guid SubjectId, Guid HomeworkId, DateTime? HomeworkDeadline);
public record ChangeHomeworkDescriptionRequest(Guid SubjectId, Guid HomeworkId, string HomeworkDescription);

public record ChangeHomeworkSourceUrlRequest(Guid SubjectId, Guid HomeworkId, List<RelatedLink> RelatedLinks);
public record ChangeHomeworkTitleRequest(Guid SubjectId, Guid HomeworkId, string HomeworkTitle);

public record CommentHomeworkRequest(string Content);
