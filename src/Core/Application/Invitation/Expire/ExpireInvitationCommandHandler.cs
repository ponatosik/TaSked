﻿using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class ExpireInvitationCommandHandler : IRequestHandler<ExpireInvitationCommand>
{
    private readonly IApplicationDbContext _context;

    public ExpireInvitationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ExpireInvitationCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var group = _context.Groups.Include(g => g.Invitations).FindOrThrow(user.GroupId.Value);
        var invitation = group.Invitations.FindOrThrow(request.InvitationId);

        invitation.Expire();
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}