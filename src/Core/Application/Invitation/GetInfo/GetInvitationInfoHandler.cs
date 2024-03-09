using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class GetInvitationInfoHandler : IRequestHandler<GetInvitationInfoQuery, Invitation>
{
    private readonly IApplicationDbContext _context;

    public GetInvitationInfoHandler(IApplicationDbContext context)
    {
        _context = context;
    }

	public Task<Invitation> Handle(GetInvitationInfoQuery request, CancellationToken cancellationToken)
	{
		var invitation = _context.Groups
			.FirstOrDefault(g => g.Invitations.Any(i => i.Id == request.InvitationId))?
			.Invitations.FindById(request.InvitationId);

		if (invitation is null)
		{
			throw new EntityNotFoundException(request.InvitationId, nameof(Invitation));
		}

		return Task.FromResult(invitation);
	}
 }