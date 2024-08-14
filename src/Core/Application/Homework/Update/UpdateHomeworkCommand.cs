using MediatR;
using TaSked.Application.Common;
using TaSked.Domain;

namespace TaSked.Application;

public record UpdateHomeworkCommand(
	 Guid UserId,
	 Guid SubjectId,
	 Guid HomeworkId,
	 Optional<string> HomeworkTitle,
	 Optional<string> HomeworkDescription,
	 Optional<DateTime?> HomeworkDeadline,
	 Optional<string?> HomeworkSourceUrl
	) : IRequest<Homework>;
