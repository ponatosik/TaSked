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
        var promoter = _context.Users.FindById(request.PromotedBy);
        if (promoter.GroupId != request.GroupId)
        {
            throw new UserIsNotGroupMemberException(request.GroupId, request.UserId);
        }
        var group = _context.Groups.Include(e => e.Members).FindById(request.GroupId);
        var user = _context.Users.FindById(request.UserId);

        user.Demote(request.Role, group);

        return Task.FromResult(group.Members.ToList());
    }
}