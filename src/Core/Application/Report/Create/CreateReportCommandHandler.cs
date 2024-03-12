using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Report>
{
    private readonly IApplicationDbContext _context;

    public CreateReportCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Report> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.FindById(user.GroupId.Value);
        
        var report = group.CreateReport(request.ReportTitle, request.ReportMessage);

        await _context.SaveChangesAsync(cancellationToken);
        return report;
    }
}