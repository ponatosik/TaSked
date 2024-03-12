using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class CreateInvitationCommandHandler : IRequestHandler<CreateInvitationCommand, Invitation>
{
    private readonly IApplicationDbContext _context;

    public CreateInvitationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Invitation> Handle(CreateInvitationCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.FindById(user.GroupId.Value);

        var invitation = group.CreateInvintation(request.InvitationCaption);

        await _context.SaveChangesAsync(cancellationToken);
        return invitation;
    }
}