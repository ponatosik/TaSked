using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record DeleteSubjectCommand(Guid UserId, Guid SubjectId) : IRequest;