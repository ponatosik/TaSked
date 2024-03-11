using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record GetAllSubjectsQuery(Guid UserId) : IRequest<List<SubjectDTO>>;