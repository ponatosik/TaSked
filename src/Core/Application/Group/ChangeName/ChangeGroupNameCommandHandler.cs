using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class ChangeGroupNameCommandHandler : IRequestHandler<ChangeGroupNameCommand, Group>
{
    private readonly IApplicationDbContext _context;

    public ChangeGroupNameCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Group> Handle(ChangeGroupNameCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.FindById(user.GroupId.Value);

        group.Name = request.GroupName;

        await _context.SaveChangesAsync(cancellationToken);
        return group;
    }
}