using TaSked.Domain;

namespace Domain.Exceptions;

public class InvitationExpiredException : DomainException
{
	public Guid InvitationId { get; private set; }
	
	private static string GenerateMessage(Invitation invitation) => 
		$"Invitation with id {invitation.Id} expired.";

	internal InvitationExpiredException(Invitation invitation) 
		: base(GenerateMessage(invitation))
	{
		InvitationId = invitation.Id;
	}
	internal InvitationExpiredException(Invitation invitation, Exception inner)
		: base(GenerateMessage(invitation), inner)
	{
		InvitationId = invitation.Id;
	}

}
