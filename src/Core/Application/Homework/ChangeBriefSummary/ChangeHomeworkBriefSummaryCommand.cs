using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record ChangeHomeworkBriefSummaryCommand(Guid UserId, Guid SubjectId, Guid HomeworkId, string? BriefSummary)
	: IRequest<Homework>;