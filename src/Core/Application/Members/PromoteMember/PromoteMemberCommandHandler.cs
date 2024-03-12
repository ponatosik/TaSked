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
        var promoter = _context.Users.FindById(request.PromotedBy);
        if (promoter.GroupId != request.GroupId)
        {
            throw new UserIsNotGroupMemberException(request.GroupId, request.UserId);
        }
        var group = _context.Groups.Include(e => e.Members).FindById(request.GroupId);
        var user = _context.Users.FindById(request.UserId);

        user.Promote(request.Role, group);

        _context.SaveChangesAsync(cancellationToken);

        return Task.FromResult(group.Members.ToList());
    }
}