namespace TaSked.Api.Requests;

public record CreateInvitationRequest(
	string? InvitationCaption,
	int? MaxActivations = null,
	DateTime? ExpirationDate = null);

public record ActivateInvitationRequest(Guid InvitationId, Guid GroupId);

public record ExpireInvitationRequest(Guid InvitationId);