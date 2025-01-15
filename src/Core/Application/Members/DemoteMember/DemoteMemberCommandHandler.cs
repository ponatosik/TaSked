using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class DemoteMemberCommandHandler : IRequestHandler<DemoteMemberCommand>
{
    private readonly IApplicationDbContext _context;

    public DemoteMemberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task Handle(DemoteMemberCommand request, CancellationToken cancellationToken)
    {
        var moderator = _context.Users.FindOrThrow(request.PromotedBy);
        if (moderator.GroupId != request.GroupId)
        {
            throw new UserIsNotGroupMemberException(request.GroupId, request.UserId);
        }
        var group = _context.Groups
            .Include(e => e.Members)
            .FindOrThrow(request.GroupId);
        var user = group.Members.FindOrThrow(request.UserId);

        user.Demote(request.Role, group);

        _context.SaveChangesAsync(cancellationToken);

        return Task.FromResult(group.Members.ToList());
    }
}