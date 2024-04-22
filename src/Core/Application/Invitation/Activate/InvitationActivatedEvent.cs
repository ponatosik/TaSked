using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record InvitationActivatedEvent(Invitation Invitation, User User, Guid GroupId) : INotification;
