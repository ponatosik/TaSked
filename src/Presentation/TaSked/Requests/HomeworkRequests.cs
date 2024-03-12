namespace TaSked.Api.Requests;

public record CreateHomeworkRequest(Guid SubjectId, string Title, string Description);
public record DeleteHomeworkRequest(Guid SubjectId, Guid HomeworkId);
public record ChangeHomeworkDeadlineRequest(Guid SubjectId, Guid HomeworkId, DateTime? HomeworkDeadline);
public record ChangeHomeworkDescriptionRequest(Guid SubjectId, Guid HomeworkId, string HomeworkDescription);
public record ChangeHomeworkSourceUrlRequest(Guid SubjectId, Guid HomeworkId, string? HomeworkSourceUrl);
public record ChangeHomeworkTitleRequest(Guid SubjectId, Guid HomeworkId, string HomeworkTitle);