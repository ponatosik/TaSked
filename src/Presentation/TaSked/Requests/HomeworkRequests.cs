namespace TaSked.Api.Requests;

public record CreateHomeworkRequest(Guid SubjectId, string Title, string Description);
