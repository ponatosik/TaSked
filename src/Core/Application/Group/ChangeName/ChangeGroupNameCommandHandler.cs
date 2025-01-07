using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class ChangeGroupNameCommandHandler : IRequestHandler<ChangeGroupNameCommand, GroupDTO>
{
    private readonly IApplicationDbContext _context;

    public ChangeGroupNameCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GroupDTO> Handle(ChangeGroupNameCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var group = _context.Groups.FindOrThrow(user.GroupId.Value);

        group.Name = request.GroupName;

        await _context.SaveChangesAsync(cancellationToken);
        return GroupDTO.From(group);
    }
}