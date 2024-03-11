using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;

namespace TaSked.Application;

public class DemoteMemberCommandHandler : IRequestHandler<PromoteMemberCommand>
{
    private readonly IApplicationDbContext _context;

    public DemoteMemberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task Handle(PromoteMemberCommand request, CancellationToken cancellationToken)
    {
        var group = _context.Groups.Include(e => e.Members).FindById(request.GroupId);
        var user = _context.Users.FindById(request.UserId);

        user.Demote(request.Role, group);

        return Task.FromResult(group.Members.ToList());
    }
}