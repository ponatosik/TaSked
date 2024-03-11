using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record BanMemberCommand(Guid BannedBy, Guid GroupId, Guid UserId) : IRequest;