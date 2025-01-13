using TaSked.Application.Data;
using MediatR;
using TaSked.Application.Exceptions;

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
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups.FindOrThrow(groupId);

        group.Name = request.GroupName;

        await _context.SaveChangesAsync(cancellationToken);
        return GroupDTO.From(group);
    }
}