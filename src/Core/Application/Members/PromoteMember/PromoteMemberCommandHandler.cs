using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class PromoteMemberCommandHandler : IRequestHandler<PromoteMemberCommand>
{
    private readonly IApplicationDbContext _context;

    public PromoteMemberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task Handle(PromoteMemberCommand request, CancellationToken cancellationToken)
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

        user.Promote(request.Role, group);

        _context.SaveChangesAsync(cancellationToken);

        return Task.FromResult(group.Members.ToList());
    }
}