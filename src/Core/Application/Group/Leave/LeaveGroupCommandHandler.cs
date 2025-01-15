using TaSked.Application.Data;
using MediatR;
using TaSked.Application.Exceptions;

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
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups.FindOrThrow(groupId);

        group.Leave(user);

        await _context.SaveChangesAsync(cancellationToken);
    }
}