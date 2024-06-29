using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record LessonDeletedEvent(Lesson Lesson, Guid GroupId) : INotification;
