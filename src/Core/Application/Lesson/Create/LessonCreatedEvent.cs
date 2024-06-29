using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record LessonCreatedEvent(Lesson Lesson, Guid GroupId) : INotification;
