using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record ReportCreatedEvent(Report Report, Guid GroupId) : INotification;
