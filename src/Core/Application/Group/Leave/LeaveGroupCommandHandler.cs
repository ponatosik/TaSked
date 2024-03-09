using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class LeaveGroupCommandHandler : IRequestHandler<LeaveGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public LeaveGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(LeaveGroupCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.FindById(user.GroupId.Value);

        group.Leave(user);

        await _context.SaveChangesAsync(cancellationToken);
    }
}