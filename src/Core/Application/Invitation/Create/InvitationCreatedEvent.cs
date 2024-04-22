using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record InvitationCreatedEvent(Invitation Invitation, Guid GroupId) : INotification;
