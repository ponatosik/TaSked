using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record CreateReportCommand(Guid UserId, string ReportTitle, string ReportMessage) : IRequest<Report>;