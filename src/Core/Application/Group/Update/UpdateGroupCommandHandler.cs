using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, GroupDTO>
{
    private readonly IApplicationDbContext _context;

    public UpdateGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GroupDTO> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.FindById(user.GroupId!.Value);

        if (request.GroupName.IsEmpty)
        {
            return GroupDTO.From(group);
        }

        group.Name = request.GroupName.Value;

        await _context.SaveChangesAsync(cancellationToken);
        return GroupDTO.From(group);
    }
}