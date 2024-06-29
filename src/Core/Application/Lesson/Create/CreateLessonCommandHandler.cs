﻿using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, Lesson>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher _eventPublisher;

    public CreateLessonCommandHandler(IApplicationDbContext context, IPublisher eventPublisher = null)
    {
        _context = context;
        _eventPublisher = eventPublisher;
    }

    public async Task<Lesson> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.Include(group => group.Subjects).FindById(user.GroupId.Value);
        var subject = group.Subjects.FindById(request.SubjectId);

        var lesson = subject.CreateLesson(request.LessonTime);

        await _context.SaveChangesAsync(cancellationToken);
        if (_eventPublisher is not null)
        {
            await _eventPublisher.Publish(new LessonCreatedEvent(lesson, group.Id), cancellationToken);
        }
        return lesson;
    }
}