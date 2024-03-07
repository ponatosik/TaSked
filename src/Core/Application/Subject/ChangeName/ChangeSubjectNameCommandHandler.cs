﻿using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class ChangeSubjectNameCommandHandler : IRequestHandler<ChangeSubjectNameCommand, Subject>
{
    private readonly IApplicationDbContext _context;

    public ChangeSubjectNameCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Subject> Handle(ChangeSubjectNameCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FirstOrDefault(user => user.Id == request.UserId);
        if (user == null)
        {
            throw new Exception("No such user found");
        }
        if (user.GroupId == null)
        {
            throw new Exception("User is not a member of a group");
        }
        if (user.Role == GroupRole.Member)
        {
            throw new Exception("No permision to create subjects");
        }

        var group = _context.Groups.FirstOrDefault(group => group.Id == user.GroupId);
        if (group == null)
        {
            throw new Exception("Unknown Group");
        }

        var subject = group.Subjects.FirstOrDefault(subject => subject.Id == request.SubjectId);
        subject.Name = request.NewSubjectName;

        await _context.SaveChangesAsync(cancellationToken);
        return subject;
    }
}