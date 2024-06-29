using MediatR;

namespace TaSked.Application;

public record SubjectDeletedEvent(SubjectDTO Subject, Guid GroupId) : INotification;
