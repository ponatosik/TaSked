using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

public class ChangeHomeworkRelatedLinksCommandHandler : IRequestHandler<ChangeHomeworkRelatedLinksCommand, Homework>
{
    private readonly IApplicationDbContext _context;

    public ChangeHomeworkRelatedLinksCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Homework> Handle(ChangeHomeworkRelatedLinksCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var group = _context.Groups.Include(group => group.Subjects)
            .ThenInclude(subject => subject.Homeworks)
            .FindOrThrow(user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty));
        var subject = group.Subjects.FindOrThrow(request.SubjectId);
        var homework = subject.Homeworks.FindOrThrow(request.HomeworkId);

        homework.RelatedLinks.Clear();
        homework.RelatedLinks.AddRange(request.Links);

        await _context.SaveChangesAsync(cancellationToken);
        return homework;
    }
}