namespace Api.Requests;

public record PromoteMemberRequest (Guid UserId);
public record DemoteMemberRequest (Guid UserId);
