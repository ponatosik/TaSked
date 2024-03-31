namespace TaSked.Api.Requests;

public record CreateInvintationRequest(string? InvitationCaption, int? MaxActivations = null, DateTime? ExpirationDate = null);
public record ActivateInvintationRequest(Guid InvitationId, Guid GroupId);
public record GetInvintationInfoRequest(Guid InvitationId);
public record ExpireInvintationRequest(Guid InvitationId);