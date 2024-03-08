using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application;

public class GetInvitationInfoHandler : IRequestHandler<GetInvitationInfoQuery, List<Invitation>>
{
    private readonly IApplicationDbContext _context;

    public GetInvitationInfoHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Invitation>> Handle(GetInvitationInfoQuery request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.FindById(user.GroupId.Value);

        return Task.FromResult(group.Invitations);
    }
}