using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record HomeworkDeletedEvent(Homework Homework, Guid GroupId) : INotification;
