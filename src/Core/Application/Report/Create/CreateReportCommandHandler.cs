using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Report>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher? _eventPublisher;

    public CreateReportCommandHandler(IApplicationDbContext context, IPublisher? eventPublisher = null)
    {
        _context = context;
        _eventPublisher = eventPublisher;
    }

    public async Task<Report> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups.FindOrThrow(groupId);
        
        var report = group.CreateReport(request.ReportTitle, request.ReportMessage);

        await _context.SaveChangesAsync(cancellationToken);
        if(_eventPublisher != null)
        {
            await _eventPublisher.Publish(new ReportCreatedEvent(report, group.Id), cancellationToken);
        }
        return report;
    }
}