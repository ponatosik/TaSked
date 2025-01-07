﻿using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class ChangeHomeworkDescriptionCommandHandler : IRequestHandler<ChangeHomeworkDescriptionCommand, Homework>
{
    private readonly IApplicationDbContext _context;

    public ChangeHomeworkDescriptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Homework> Handle(ChangeHomeworkDescriptionCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var group = _context.Groups.Include(group => group.Subjects).
            ThenInclude(subject => subject.Homeworks)
            .FindOrThrow(user.GroupId.Value);
        var subject = group.Subjects.FindOrThrow(request.SubjectId);
        var homework = subject.Homeworks.FindOrThrow(request.HomeworkId);

        homework.Description = request.HomeworkDescription;

        await _context.SaveChangesAsync(cancellationToken);
        return homework;
    }
}