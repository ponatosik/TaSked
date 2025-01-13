using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class ChangeHomeworkSourceUrlCommandHandler : IRequestHandler<ChangeHomeworkSourceUrlCommand, Homework>
{
    private readonly IApplicationDbContext _context;

    public ChangeHomeworkSourceUrlCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Homework> Handle(ChangeHomeworkSourceUrlCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var group = _context.Groups.Include(group => group.Subjects)
            .ThenInclude(subject => subject.Homeworks)
            .FindOrThrow(user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty));
        var subject = group.Subjects.FindOrThrow(request.SubjectId);
        var homework = subject.Homeworks.FindOrThrow(request.HomeworkId);

        homework.SourceUrl = request.HomeworkSourceUrl;

        await _context.SaveChangesAsync(cancellationToken);
        return homework;
    }
}