using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record ChangeSubjectNameCommand(Guid UserId, Guid SubjectId, string NewSubjectName) : IRequest<Subject>;