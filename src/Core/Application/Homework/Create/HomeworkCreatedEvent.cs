using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record HomeworkCreatedEvent(Homework Homework, Guid GroupId) : INotification;
