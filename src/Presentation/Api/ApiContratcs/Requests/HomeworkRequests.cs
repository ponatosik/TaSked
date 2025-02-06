using TaSked.Domain;

namespace TaSked.Api.Requests;

public record CreateHomeworkRequest(
	string Title,
	string Description,
	DateTime? Deadline = null,
	List<RelatedLink>? RelatedLinks = null);

public record ChangeHomeworkDeadlineRequest(DateTime? HomeworkDeadline);

public record ChangeHomeworkDescriptionRequest(string HomeworkDescription);

public record ChangeHomeworkRelatedLinksRequest(List<RelatedLink> RelatedLinks);

public record ChangeHomeworkBriefSummaryRequest(string BriefSummary);

public record ChangeHomeworkTitleRequest(string HomeworkTitle);

public record CommentHomeworkRequest(string Content);
