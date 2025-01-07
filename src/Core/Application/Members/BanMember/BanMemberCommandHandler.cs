using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class BanMemberCommandHandler : IRequestHandler<BanMemberCommand>
{
    private readonly IApplicationDbContext _context;

    public BanMemberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task Handle(BanMemberCommand request, CancellationToken cancellationToken)
    {
        var promoter = _context.Users.FindOrThrow(request.BannedBy);
        if (promoter.GroupId != request.GroupId)
        {
            throw new UserIsNotGroupMemberException(request.GroupId, request.UserId);
        }
        var group = _context.Groups.Include(e => e.Members).FindOrThrow(request.GroupId);
        var user = _context.Users.FindOrThrow(request.UserId);

        group.Leave(user);

        _context.SaveChangesAsync(cancellationToken);

        return Task.FromResult(group.Members.ToList());
    }
}