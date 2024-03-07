using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record GetAllLessonsInDateRangeQuery(Guid UserId, DateTime StartDate, DateTime EndDate) : IRequest<List<Lesson>>;