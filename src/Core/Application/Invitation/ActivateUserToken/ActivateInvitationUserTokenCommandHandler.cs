using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class ActivateInvitationUserTokenCommandHandler : IRequestHandler<ActivateInvitationUserTokenCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtProvider _jwtProvider;

    public ActivateInvitationUserTokenCommandHandler(IApplicationDbContext context,  IJwtProvider jwtProvider)
    {
        _context = context;
        _jwtProvider = jwtProvider;
    }

    public async Task<string> Handle(ActivateInvitationUserTokenCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.FindById(request.GroupId);
        var invitation = group.Invitations.FindById(request.InvitationId);

        group.JoinByInvintation(invitation, user);

        await _context.SaveChangesAsync(cancellationToken);

        string token = _jwtProvider.Generate(user);
        return token;
    }
}