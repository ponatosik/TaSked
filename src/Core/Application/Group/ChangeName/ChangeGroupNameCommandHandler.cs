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
        var user = _context.Users.FirstOrDefault(user => user.Id == request.UserId);
        if (user == null)
        {
            throw new Exception("No such user found");
        }
        if (user.GroupId == null)
        {
            throw new Exception("User is not a member of a group");
        }
        if (user.Role == GroupRole.Member)
        {
            throw new Exception("No permision to create subjects");
        }

        var group = _context.Groups.FirstOrDefault(group => group.Id == user.GroupId);
        if (group == null)
        {
            throw new Exception("Unknown Group");
        }

        group.Name = request.GroupName;

        await _context.SaveChangesAsync(cancellationToken);
        return group;
    }
}