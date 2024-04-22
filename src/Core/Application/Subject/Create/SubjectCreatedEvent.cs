using MediatR;

namespace TaSked.Application;

public record SubjectCreatedEvent(SubjectDTO Subject, Guid GroupId) : INotification;
