using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record GetAllReportQuery(Guid UserId) : IRequest<List<Report>>;