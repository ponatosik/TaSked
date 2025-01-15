using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups.FindOrThrow(groupId);

        _context.Groups.Remove(group);
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}